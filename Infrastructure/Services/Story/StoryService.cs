using Domain;
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

        public async Task<Story> GetStoryById(string storyId)
        {
            return await _storyRepository.GetAll().FirstOrDefaultAsync(s => s.StoryId == storyId);
        }

        public async Task<Story> AddStory(Story story)
        {
            return await _storyRepository.AddAsync(story);
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
    }
}
