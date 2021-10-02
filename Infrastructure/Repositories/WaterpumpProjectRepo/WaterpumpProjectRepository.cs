using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.DBContext;

namespace Infrastructure.Repositories.WaterpumpProjectRepo
{
    public class WaterpumpProjectRepository : CosmosRepository<WaterPumpProject>
    {
        public WaterpumpProjectRepository(IkwambeContext ikambeContext) : base(ikambeContext)
        {

        }
        public override IEnumerable<WaterPumpProject> GetAll()
        {
            try
            {
                return _ikambeContext.WaterpumpProject;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving project. {ex.Message}");
            }
        }

        public override WaterPumpProject GetById(string projectId)
        {
            try
            {
                return _ikambeContext.WaterpumpProject.FirstOrDefault(p => p.ProjectId == projectId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving project {projectId}. {ex.Message}");
            }
        }
    }
}
