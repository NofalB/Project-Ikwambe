using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WaterpumpProjectService : IWaterpumpProjectService
    {
        private readonly CosmosRepository<WaterPumpProject> _waterPumpProjectRepository;

        public WaterpumpProjectService(CosmosRepository<WaterPumpProject> waterpumpProjectRepository)
        {
            _waterPumpProjectRepository = waterpumpProjectRepository;
        }

        public async Task AddWaterpumpProject(WaterPumpProject waterPumpProject)
        {
            await _waterPumpProjectRepository.AddAsync(waterPumpProject);
        }

        public void DeleteWaterPumpProject(string projectId)
        {
            WaterPumpProject waterPumpProject = GetWaterPumpProjectById(projectId);
            _waterPumpProjectRepository.Delete(waterPumpProject);
        }

        public WaterPumpProject GetWaterPumpProjectById(string projectId)
        {
            return _waterPumpProjectRepository.GetById(projectId);
        }

        public IEnumerable<WaterPumpProject> GetAllWaterPumpProjects()
        {
            return _waterPumpProjectRepository.GetAll().ToList();
        }

        public WaterPumpProject UpdateWaterPumpProject(WaterPumpProject waterPumProject)
        {
            return _waterPumpProjectRepository.Update(waterPumProject);
        }
    }
}
