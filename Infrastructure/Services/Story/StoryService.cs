﻿using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StoryService : IStoryService
    {
        private readonly ICosmosRepository<Story> _storyRepository;

        public StoryService(ICosmosRepository<Story> storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            return await _storyRepository.GetAll().ToListAsync();
        }

        public async Task<Story> GetStoryById(Guid storyId)
        {
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.StoryId == storyId);
        }
        private async Task<Story> GetStoryByTitle(string title)
        {
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.Title == title);
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

        public async Task DeleteStory(Guid storyId)
        {
            Story story = await GetStoryById(storyId);
            await _storyRepository.Delete(story);
        }
    }
}
