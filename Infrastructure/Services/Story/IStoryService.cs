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
        IEnumerable<Story> GetAllStories();

        Story GetStoryById(string storyId);

        Task AddStory(Story story);

        Story UpdateStory(Story story);

        void DeleteStory(string storyId);
    }
}
