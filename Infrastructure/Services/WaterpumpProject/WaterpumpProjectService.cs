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

        public async Task<WaterpumpProject> GetWaterPumpProjectById(string projectId)
        {
            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == projectId);
        }

        public async Task <WaterpumpProject> GetWaterpumpByProjectName(string nameOfProject)
        {
            //return await _waterpumpProjectRepository.GetAll().SingleOrDefault(s => s.NameOfProject == nameOfProject);
            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(s => s.NameOfProject == nameOfProject);
        }

        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO)
        {
            string newId = Guid.NewGuid().ToString();
            
           // Console.WriteLine(GetWaterpumpByProjectName(waterpumpProjectDTO.NameOfProject).ToString());
           // if(GetWaterpumpByProjectName(waterpumpProjectDTO.NameOfProject) != null)
           // {
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
           // }
            /*else
            {
               throw new Exception("project already exist");
            }*/
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumProject)
        {
            return await _waterpumpProjectRepository.Update(waterpumProject);
        }

        public async Task DeleteWaterpumpProjectAsync(string projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterpumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
