using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StoryService : IStoryService
    {
        private readonly CosmosRepository<Story> _storyRepository;

        public StoryService(CosmosRepository<Story> storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task AddStory(Story story)
        {
            await _storyRepository.AddAsync(story);
        }

        public void DeleteStory(string storyId)
        {
            Story story = GetStoryById(storyId);
            _storyRepository.Delete(story);
        }

        public IEnumerable<Story> GetAllStories()
        {
            return _storyRepository.GetAll().ToList();
        }

        public Story GetStoryById(string storyId)
        {
            return _storyRepository.GetById(storyId);
        }

        public Story UpdateStory(Story story)
        {
            return _storyRepository.Update(story);
        }
    }
}
