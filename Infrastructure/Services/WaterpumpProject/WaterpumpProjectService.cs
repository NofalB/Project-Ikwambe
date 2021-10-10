using Domain;
using Domain.DTO;
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
        private readonly ICosmosRepository<WaterpumpProject> _waterpumpProjectRepository;
        private readonly ICosmosRepository<WaterpumpProjectDTO> _waterpumpProjectDTORepository;

        public WaterpumpProjectService(ICosmosRepository<WaterpumpProject> waterpumpProjectRepository)
        {
            _waterpumpProjectRepository = waterpumpProjectRepository;
        }

        public async Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects()
        {
            return await _waterpumpProjectRepository.GetAll().ToListAsync();
        }

        public async Task<WaterpumpProject> GetWaterPumpProjectById(string projectId)
        {
            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == projectId);
        }

        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO)
        {
            string newId = Guid.NewGuid().ToString();
            WaterpumpProject wp = new WaterpumpProject()
            {
                ProjectId = newId,
                NameOfProject = waterpumpProjectDTO.NameOfProject,
                RatedPower = waterpumpProjectDTO.RatedPower,
                FlowRate = waterpumpProjectDTO.FlowRate,
                PartitionKey = newId,
                Coordinates = waterpumpProjectDTO.Coordinates,
                CurrentDonation = waterpumpProjectDTO.CurrentDonation,
                TargetGoal = waterpumpProjectDTO.TargetGoal,
                StartDate = waterpumpProjectDTO.StartDate,
                EndDate = waterpumpProjectDTO.EndDate
            };

            return await _waterpumpProjectRepository.AddAsync(wp);
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterPumProject)
        {
            return await _waterpumpProjectRepository.Update(waterPumProject);
        }

        public async Task DeleteWaterPumpProject(string projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterpumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
