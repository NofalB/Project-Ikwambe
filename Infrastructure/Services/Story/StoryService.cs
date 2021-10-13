﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        private readonly ICosmosRepository<Story> _storyRepository;

        public StoryService(ICosmosRepository<Story> storyRepository, IOptions<BlobCredentialOptions> options)
        {
            _storyRepository = storyRepository;
            _blobCredentialOptions = options.Value;

            blobServiceClient = new BlobServiceClient(_blobCredentialOptions.ConnectionString);
            containerClient = blobServiceClient.GetBlobContainerClient(_blobCredentialOptions.ContainerName);
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            return await _storyRepository.GetAll().ToListAsync();
        }

        public async Task<Story> GetStoryById(string storyId)
        {
            Guid id = Guid.Parse(storyId);
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.StoryId == id);
        }
        private async Task<Story> GetStoryByTitle(string title)
        {
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.Title == title);
        }

        private IQueryable<Story> GetStoryByAuthor(string Author)
        {
            return _storyRepository.GetAll().Where(s => s.Author == Author);
        }

        private async Task<Story> GetStoryByDate(DateTime dateTime)
        {
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.PublishDate == dateTime);
        }

        public IQueryable<Story> GetStoryByQuery(string author, string publishDate)
        {
            IQueryable<Story> story = _storyRepository.GetAll();

            if(author!=null)
            {
                story = story.Where(s => s.Author == author);
            }
            if(publishDate !=null)
            {
                DateTime time = DateTime.Parse(publishDate);
                publishDate += "T23:59:59";
                DateTime time_end = DateTime.Parse(publishDate);
                story = story.Where(s => s.PublishDate > time && s.PublishDate < time_end);
            }

            return story;
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

                return await _storyRepository.AddAsync(story);
            }
            else
            {
                throw new Exception("The story already exist");
            }
        }

        public async Task<Story> UpdateStory(Story story)
        {
            return await _storyRepository.Update(story);
        }

        public async Task DeleteStory(string storyId)
        {
            Story story = await GetStoryById(storyId);
            await _storyRepository.Delete(story);
        }


        public async Task UploadImage(string storyId, Stream fileStream, string fileName)
        {
            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "image/png" });

            var imageUrl = GetServiceSasUriForBlob(blobClient);

            var story = await GetStoryById(storyId);
            story.ImageURL = imageUrl;

            await UpdateStory(story);
        }

        private string GetServiceSasUriForBlob(BlobClient blobClient, string storedPolicyName = null)
        {
            string imageUrl = "";

            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                imageUrl = sasUri.AbsoluteUri;
            }

            return imageUrl;
        }
    }
}
