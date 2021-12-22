using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Domain;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Helpers;
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
            ICosmosWriteRepository<Story> storyWriteRepository)
        {
            _storyReadRepository = storyReadRepository;
            _storyWriteRepository = storyWriteRepository;
        }

        public StoryService(ICosmosReadRepository<Story> storyReadRepository,
            ICosmosWriteRepository<Story> storyWriteRepository, IOptions<BlobCredentialOptions> options)
        {
            _storyReadRepository = storyReadRepository;
            _storyWriteRepository = storyWriteRepository;
            _blobCredentialOptions = options.Value;

            blobServiceClient = new BlobServiceClient("BlobStorageConnectionString".GetSecretValue().GetAwaiter().GetResult());
            containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BlobCredentialOptions:ContainerName", EnvironmentVariableTarget.Process));
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            return await _storyReadRepository.GetAll().ToListAsync();
        }

        public async Task<Story> GetStoryById(string storyId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(storyId) ? Guid.Parse(storyId) : throw new ArgumentNullException("No story ID was provided.");
                var story = await _storyReadRepository.GetAll().FirstOrDefaultAsync(s => s.StoryId == id);

                if (story == null)
                {
                    throw new InvalidOperationException($"No story exists with the ID {storyId}");
                }
                return story;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid story Id {storyId} provided.");
            }
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
                storyResultList = story.Where(s =>s.Author == author).ToList();
            }
            if(publishDate != null)
            {
                DateTime publishDt;

                if(!DateTime.TryParseExact(publishDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDt))
                {
                    throw new InvalidOperationException("Invalid date provided");
                }

                publishDt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                storyResultList = storyResultList.Count != 0 ?
                    story.Where(d => d.PublishDate.Date == publishDt.Date && d.Author == author).ToList()
                    : storyResultList = story.Where(d => d.PublishDate.Date == publishDt.Date).ToList();

                return storyResultList.Count != 0 ? storyResultList : new List<Story>();
            }

            return storyResultList.Count !=0 ? storyResultList : story;
        }


        public async Task<Story> AddStory(StoryDTO storyDTO)
        {
            if (storyDTO == null)
            {
                throw new NullReferenceException($"{nameof(storyDTO)} cannot be null.");
            }

            var story = await GetStoryByTitle(storyDTO.Title);

            if (story != null)
            {
                throw new InvalidOperationException($"This story with title '{storyDTO.Title}' already exists.");
            }

            Story newStory = new Story(
                Guid.NewGuid(),
                storyDTO.Title,
                storyDTO.PublishDate != default(DateTime) ? storyDTO.PublishDate : DateTime.Now,
                !string.IsNullOrEmpty(storyDTO.Summary) ? storyDTO.Summary : throw new InvalidOperationException($"Invalid {nameof(storyDTO.Summary)} provided."),
                !string.IsNullOrEmpty(storyDTO.Description) ? storyDTO.Description : throw new InvalidOperationException($"Invalid {nameof(storyDTO.Description)} provided."),
                !string.IsNullOrEmpty(storyDTO.Author) ? storyDTO.Author : throw new InvalidOperationException($"Invalid {nameof(storyDTO.Author)} provided.")
            );

            return await _storyWriteRepository.AddAsync(newStory);
        }

        public async Task<Story> UpdateStory(Story story)
        {
            return await _storyWriteRepository.Update(story);
        }

        public async Task<Story> UpdateStory(StoryDTO storyDto, string storyId)
        {
            var existingStory = await GetStoryById(storyId);
            if (existingStory != null)
            {
                existingStory.Title = !string.IsNullOrEmpty(storyDto.Title) ? storyDto.Title : throw new InvalidOperationException($"Invalid {nameof(storyDto.Title)} provided.");
                existingStory.PublishDate = storyDto.PublishDate != default(DateTime) ? storyDto.PublishDate : throw new InvalidOperationException($"Invalid {nameof(storyDto.PublishDate)} provided.");
                existingStory.Summary = !string.IsNullOrEmpty(storyDto.Summary) ? storyDto.Summary : throw new InvalidOperationException($"Invalid {nameof(storyDto.Summary)} provided.");
                existingStory.Description = !string.IsNullOrEmpty(storyDto.Description) ? storyDto.Description : throw new InvalidOperationException($"Invalid {nameof(storyDto.Description)} provided.");
                existingStory.Author = !string.IsNullOrEmpty(storyDto.Author) ? storyDto.Author : throw new InvalidOperationException($"Invalid {nameof(storyDto.Author)} provided.");
                //existingStory.PartitionKey = storyDto.Author;

                return await _storyWriteRepository.Update(existingStory);
            }
            else
            {
                throw new InvalidOperationException("The story ID provided does not exist");
            }

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
               
                var storyImage = new StoryImage(file.Name, blobUrl);
                story.StoryImages.Add(storyImage);

                await UpdateStory(story);
            }
            else
            {
                throw new InvalidOperationException("Invalid content type. Media type not supported. Upload a valid image of type jpeg,bmp or png.");
            }

        }

    }
}
