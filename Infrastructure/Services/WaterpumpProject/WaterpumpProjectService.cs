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
            //add geo coordinate here
            Coordinates c = new Coordinates(
                waterPumpProject.Coordinates.CoordinateId + 1,
                waterPumpProject.Coordinates.LocationName,
                waterPumpProject.Coordinates.Longitude,
                waterPumpProject.Coordinates.Latitude
                );
            await _waterPumpProjectRepository.AddAsync(waterPumpProject);
        }

        public void DeleteWaterPumpProject(string projectId)
        {
            WaterPumpProject waterPumpProject = GetWaterPumpProjectById(projectId);
            _waterPumpProjectRepository.Delete(waterPumpProject);
        }

        public WaterPumpProject GetWaterPumpProjectById(string projectId)
        {
            //return _waterPumpProjectRepository.GetById(projectId);
            return null;
        }

        public IEnumerable<WaterPumpProject> GetAllWaterPumpProjects()
        {
            return _waterPumpProjectRepository.GetAll().ToList();
        }

        public WaterPumpProject UpdateWaterPumpProject(WaterPumpProject waterPumProject)
        {
            // return _waterPumpProjectRepository.Update(waterPumProject);
            return null;
        }
    }
}
