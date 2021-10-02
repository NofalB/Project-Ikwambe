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
        IEnumerable<WaterPumpProject> GetAllWaterPumpProjects();

        WaterPumpProject GetWaterPumpProjectById(string projectId);

        Task AddWaterpumpProject(WaterPumpProject waterPumpProject);

        WaterPumpProject UpdateWaterPumpProject(WaterPumpProject waterPumProject);

        void DeleteWaterPumpProject(string projectId);
    }
}
