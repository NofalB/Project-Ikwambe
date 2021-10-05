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
        Task<IEnumerable<WaterPumpProject>> GetAllWaterPumpProjects();

        Task<WaterPumpProject> GetWaterPumpProjectById(string projectId);

        Task<WaterPumpProject> AddWaterpumpProject(WaterPumpProject waterPumpProject);

        Task<WaterPumpProject> UpdateWaterPumpProject(WaterPumpProject waterPumProject);

        Task DeleteWaterPumpProject(string projectId);
    }
}
