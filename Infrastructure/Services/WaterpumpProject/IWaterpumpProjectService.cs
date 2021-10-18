using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;

namespace Infrastructure.Services
{
    public interface IWaterpumpProjectService
    {
        Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects();

        Task<WaterpumpProject> GetWaterPumpProjectById(string projectId);

        Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO);

        Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumpProject);
        Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumpProject, string projectId);

        Task DeleteWaterpumpProjectAsync(string projectId);

        List<WaterpumpProject> GetWaterpumpProjectByQuery(string projectType, string projectName);
    }
}
