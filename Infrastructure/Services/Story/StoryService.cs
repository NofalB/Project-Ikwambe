using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Domain;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StoryService : IStoryService
    {

        private BlobServiceClient blobServiceClient;
        private BlobContainerClient containerClient;
        private readonly BlobCredentialOptions _blobCredentialOptions;

        private readonly ICosmosReadRepository<Story> _storyReadRepository;
        private readonly ICosmosWriteRepository<Story> _storyWriteRepository;

        public StoryService(ICosmosReadRepository<Story> storyReadRepository,
            ICosmosWriteRepository<Story> storyWriteRepository, IOptions<BlobCredentialOptions> options)
        {
            _storyReadRepository = storyReadRepository;
            _storyWriteRepository = storyWriteRepository;
            _blobCredentialOptions = options.Value;

            blobServiceClient = new BlobServiceClient(_blobCredentialOptions.ConnectionString);
            containerClient = blobServiceClient.GetBlobContainerClient(_blobCredentialOptions.ContainerName);
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            return await _storyReadRepository.GetAll().ToListAsync();
        }

        public async Task<Story> GetStoryById(string storyId)
        {
            Guid id = Guid.Parse(storyId);
            return await _storyReadRepository.GetAll().FirstOrDefaultAsync(s => s.StoryId == id);
        }
        private async Task<Story> GetStoryByTitle(string title)
        {
            return await _storyReadRepository.GetAll().FirstOrDefaultAsync(s => s.Title == title);
        }

        public List<Story> GetStoryByQuery(string author, string publishDate)
        {
            List<Story> story = _storyReadRepository.GetAll().ToList();
            List<Story> storyResultList = new List<Story>();

            if(author != null)
            {
                storyResultList.AddRange(story.Where(s =>
                {
                    try
                    {
                        return s.Author == author;
                    }
                    catch
                    {
                        throw new InvalidOperationException("Author provided either does not exist or ");
                    }
                }));
                //story = story.Where(s => s.Author == author);
            }
            if(publishDate != null)
            {
                DateTime publishDay;

                if(!DateTime.TryParseExact(publishDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDay))
                {
                    throw new InvalidOperationException("Invalid date provided");
                }

                publishDay.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                storyResultList.AddRange(story.Where(s => s.PublishDate == publishDay.Date).ToList());
                //DateTime time = DateTime.Parse(publishDate);
                //publishDate += "T23:59:59";
                //DateTime time_end = DateTime.Parse(publishDate);
                //story = story.Where(s => s.PublishDate > time && s.PublishDate < time_end);
            }

            return storyResultList.Count !=0 ? storyResultList : story;
        }


        public async Task<Story> AddStory(StoryDTO storyDTO)
        {
            if(await GetStoryByTitle(storyDTO.Title) == null)
            {
                Story story = new Story(
                Guid.NewGuid(),
                storyDTO.Title,
                storyDTO.ImageURL,
                storyDTO.PublishDate,
                storyDTO.Summary,
                storyDTO.Description,
                storyDTO.Author);

                return await _storyWriteRepository.AddAsync(story);
            }
            else
            {
                throw new Exception("The story already exist");
            }
        }

        public async Task<Story> UpdateStory(Story story)
        {
            return await _storyWriteRepository.Update(story);
        }

        public async Task<Story> UpdateStory(Story story, string storyId)
        {
            if(await GetStoryById(storyId) == null)
            {
                throw new InvalidOperationException("The story ID provided does not exist");
            }
            return await _storyWriteRepository.Update(story);
        }

        public async Task DeleteStory(string storyId)
        {
            Story story = await GetStoryById(storyId);
            await _storyWriteRepository.Delete(story);
        }


        public async Task UploadImage(string storyId, FilePart file)
        {
            if (file.ContentType == "image/jpeg" || file.ContentType == "image/bmp" || file.ContentType == "image/png")
            {
                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(file.Name);

                // Upload the file
                await blobClient.UploadAsync(file.Data, new BlobHttpHeaders { ContentType = file.ContentType });

                //get the URL of the uploaded image
                var blobUrl = blobClient.Uri.AbsoluteUri;

                var story = await GetStoryById(storyId);

                //set the new url for the existing story
                story.ImageURL = blobUrl;

                await UpdateStory(story);
            }
            else
            {
                throw new Exception("Invalid content type. Media type not supported. Upload a valid image of type jpeg,bmp or png.");
            }

        }

    }
}
