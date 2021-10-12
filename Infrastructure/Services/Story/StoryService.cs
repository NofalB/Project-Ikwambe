using Domain;
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
    }
}
