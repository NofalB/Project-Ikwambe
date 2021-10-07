using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Services
{
    public interface IWaterpumpProjectService
    {
        Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects();

        Task<WaterpumpProject> GetWaterPumpProjectById(string projectId);

        Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProject waterPumpProject);

        Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterPumProject);

        Task DeleteWaterPumpProject(string projectId);
    }
}
