using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<Story>> GetAllStories();

        Task<Story> GetStoryById(string storyId);

        Task<Story> AddStory(Story story);

        Task<Story> UpdateStory(Story story);

        Task DeleteStory(string storyId);
    }
}
