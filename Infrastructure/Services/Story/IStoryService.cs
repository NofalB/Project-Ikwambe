using Domain;
using Domain.DTO;
using HttpMultipartParser;
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
        Task<Story> UpdateStory(Story story, string userId);

        Task DeleteStory(string storyId);

        List<Story> GetStoryByQuery(string author, string publishDate);
        Task UploadImage(string storyId, FilePart file);
    }
}
