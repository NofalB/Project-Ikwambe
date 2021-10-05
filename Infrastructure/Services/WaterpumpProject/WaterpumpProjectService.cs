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
        private readonly ICosmosRepository<WaterPumpProject> _waterPumpProjectRepository;

        public WaterpumpProjectService(ICosmosRepository<WaterPumpProject> waterpumpProjectRepository)
        {
            _waterPumpProjectRepository = waterpumpProjectRepository;
        }

        public async Task<IEnumerable<WaterPumpProject>> GetAllWaterPumpProjects()
        {
            return await _waterPumpProjectRepository.GetAll().ToListAsync();
        }

        public async Task<WaterPumpProject> GetWaterPumpProjectById(string projectId)
        {
            return await _waterPumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == projectId);
        }

        public async Task<WaterPumpProject> AddWaterpumpProject(WaterPumpProject waterPumpProject)
        {
            return await _waterPumpProjectRepository.AddAsync(waterPumpProject);
        }

        public async Task<WaterPumpProject> UpdateWaterPumpProject(WaterPumpProject waterPumProject)
        {
            return await _waterPumpProjectRepository.Update(waterPumProject);
        }

        public async Task DeleteWaterPumpProject(string projectId)
        {
            WaterPumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterPumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
