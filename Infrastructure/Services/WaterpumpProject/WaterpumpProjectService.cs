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

        public WaterpumpProjectService(ICosmosRepository<WaterpumpProject> waterpumpProjectRepository)
        {
            _waterpumpProjectRepository = waterpumpProjectRepository;
        }

        public async Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects()
        {
            return await _waterpumpProjectRepository.GetAll().ToListAsync();
        }

        public async Task<WaterpumpProject> GetWaterPumpProjectById(Guid projectId)
        {
            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == projectId);
        }

 

        private async Task<WaterpumpProject> GetWaterpumpProjectByName(string projectName)
        {
            WaterpumpProject p = await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.NameOfProject == projectName);
            return p;
            
        }

        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO)
        {            
            if(await GetWaterpumpProjectByName(waterpumpProjectDTO.NameOfProject) == null)
            {
                WaterpumpProject wp = new WaterpumpProject()
                {
                    ProjectId = Guid.NewGuid(),
                    NameOfProject = waterpumpProjectDTO.NameOfProject,
                    RatedPower = waterpumpProjectDTO.RatedPower,
                    FlowRate = waterpumpProjectDTO.FlowRate,
                    Coordinates = waterpumpProjectDTO.Coordinates,
                    CurrentDonation = waterpumpProjectDTO.CurrentDonation,
                    TargetGoal = waterpumpProjectDTO.TargetGoal,
                    StartDate = waterpumpProjectDTO.StartDate,
                    EndDate = waterpumpProjectDTO.EndDate
                };
                return await _waterpumpProjectRepository.AddAsync(wp);
            }
            else
            {
                throw new ("Project already exist");
            }
            
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumProject)
        {
            return await _waterpumpProjectRepository.Update(waterpumProject);
        }

        public async Task DeleteWaterpumpProjectAsync(Guid projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterpumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
