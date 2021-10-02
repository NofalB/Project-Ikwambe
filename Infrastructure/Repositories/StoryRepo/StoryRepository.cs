using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StoryRepo
{
    public class StoryRepository : CosmosRepository<Story>
    {
        public StoryRepository(IkwambeContext ikwambeContext): base(ikwambeContext)
        {

        }
        public override IEnumerable<Story> GetAll()
        {
            try
            {
                return _ikambeContext.Stories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving stories. {ex.Message}");
            }
        }

        public override Story GetById(string storyId)
        {
            try
            {
                return _ikambeContext.Stories.FirstOrDefault(s => s.StoryId == storyId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving donation {storyId}. {ex.Message}");
            }
        }
    }
}
