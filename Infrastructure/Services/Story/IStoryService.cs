using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<Story>> GetAllStories();

        Task<Story> GetStoryById(string storyId);

        Task<Story> AddStory(StoryDTO story);

        Task<Story> UpdateStory(Story story);

        Task DeleteStory(string storyId);

        Task UploadImage(string storyId, Stream fileStream, string fileName);
    }
}
