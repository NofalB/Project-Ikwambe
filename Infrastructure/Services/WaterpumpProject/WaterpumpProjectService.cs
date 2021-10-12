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
            Guid id = Guid.Parse(projectId);
            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == id);
        }

        public  async Task<WaterpumpProject> GetWaterpumpProjectByName(string projectName)
        {
            WaterpumpProject p = await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(w => w.NameOfProject == projectName);
            return p;
        }

        public async Task<WaterpumpProject> GetWaterPumpByProjectType(string projectType)
        {
            ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

            return await _waterpumpProjectRepository.GetAll().FirstOrDefaultAsync(p => p.ProjectType == pt);
        }

        public IQueryable<WaterpumpProject>GetWaterPumpByProjectType1(string projectType)
        {
            ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

            return _waterpumpProjectRepository.GetAll().Where(p => p.ProjectType == pt);
        }

        public IQueryable<WaterpumpProject> GetWaterPumpProjectByQuery(string projectType, string projectName)
        {
            IQueryable<WaterpumpProject> waterpumpProjects = _waterpumpProjectRepository.GetAll();


            if(projectType != null)
            {
                ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

                waterpumpProjects = waterpumpProjects.Where(p => p.ProjectType == pt);
            }
            if(projectName !=null)
            {
                waterpumpProjects = waterpumpProjects.Where(p => p.NameOfProject == projectName);
            }
            return waterpumpProjects;



           /* if (projectType != null && projectName == null)
            {
                ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

                return _waterpumpProjectRepository.GetAll().Where(p => p.ProjectType == pt);
            }
            else if (projectType == null && projectName != null)
            {
                return _waterpumpProjectRepository.GetAll().Where(p => p.NameOfProject == projectName);
            }
            *//*if (projectName != null)
            {
                return _waterpumpProjectRepository.GetAll().Where(p => p.NameOfProject == projectName);
            }*//*
            else
            {
                return _waterpumpProjectRepository.GetAll();
                throw new Exception("get all loop");
            }*/
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
                    EndDate = waterpumpProjectDTO.EndDate,
                    ProjectType = waterpumpProjectDTO.ProjectType,
                    PartitionKey = waterpumpProjectDTO.ProjectType.ToString()
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

        public async Task DeleteWaterpumpProjectAsync(string projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterpumpProjectRepository.Delete(waterPumpProject);
        }
    }
}
