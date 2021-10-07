using Domain;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WaterpumpProjectService : IWaterpumpProjectService
    {
        private readonly ICosmosRepository<WaterpumpProject> _waterPumpProjectRepository;

        public WaterpumpProjectService(ICosmosRepository<WaterpumpProject> waterpumpProjectRepository)
        {
            _waterPumpProjectRepository = waterpumpProjectRepository;
        }

        public async Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects()
        {
            return await _waterPumpProjectRepository.GetAll().ToListAsync();
        }

        public async Task<WaterpumpProject> GetWaterPumpProjectById(string projectId)
        {
            return await _waterPumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == projectId);
        }

        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProject waterPumpProject)
        {
            return await _waterPumpProjectRepository.AddAsync(waterPumpProject);
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterPumProject)
        {
            return await _waterPumpProjectRepository.Update(waterPumProject);
        }

        public async Task DeleteWaterPumpProject(string projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterPumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
