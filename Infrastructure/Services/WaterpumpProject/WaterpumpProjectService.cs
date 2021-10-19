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
        private readonly ICosmosReadRepository<WaterpumpProject> _waterpumpProjectReadRepository;
        private readonly ICosmosWriteRepository<WaterpumpProject> _waterpumpProjectWriteRepository;

        public WaterpumpProjectService(ICosmosReadRepository<WaterpumpProject> waterpumpProjectReadRepository,
            ICosmosWriteRepository<WaterpumpProject> waterpumpProjectWriteRepository)
        {
            _waterpumpProjectReadRepository = waterpumpProjectReadRepository;
            _waterpumpProjectWriteRepository = waterpumpProjectWriteRepository;
        }

        public async Task<IEnumerable<WaterpumpProject>> GetAllWaterPumpProjects()
        {
            return await _waterpumpProjectReadRepository.GetAll().ToListAsync();
        }

        public async Task<WaterpumpProject> GetWaterPumpProjectById(string projectId)
        {
            Guid id = Guid.Parse(projectId);
            var test = await _waterpumpProjectReadRepository.GetAll().ToListAsync();

            var project = await _waterpumpProjectReadRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == id);
            
            if(project == null)
            {
                throw new ArgumentNullException("No Id Found");
            }

            return project;
        }

        public async Task<WaterpumpProject> GetWaterpumpProjectByName(string projectName)
        {
            WaterpumpProject p = await _waterpumpProjectReadRepository.GetAll().FirstOrDefaultAsync(w => w.NameOfProject == projectName);
            return p;
        }

        public async Task<WaterpumpProject> GetWaterPumpByProjectType(string projectType)
        {
            ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

            return await _waterpumpProjectReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProjectType == pt);
        }

        public List<WaterpumpProject> GetWaterpumpProjectByQuery(string projectType, string projectName)
        {
            List<WaterpumpProject> resultList = new List<WaterpumpProject>();
            List<WaterpumpProject> waterpumpProjects = _waterpumpProjectReadRepository.GetAll().ToList();

            if (projectType != null)
            {
                ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

                resultList.AddRange(waterpumpProjects.Where(p =>
                {
                    try
                    {
                        return p.ProjectType == pt;
                    }
                    catch
                    {
                        throw new InvalidOperationException("Invalid ProjectType Provided");
                    }
                }));
                //waterpumpProjects = waterpumpProjects.Where(p => p.ProjectType == pt);
            }
            if (projectName != null)
            {
                resultList.AddRange(waterpumpProjects.Where(p =>
                {
                    try
                    {
                        return p.NameOfProject == projectName;
                    }
                    catch
                    {
                        throw new InvalidOperationException("The Project name you have provided is invalid or does not exist");

                    }
                }));
                //waterpumpProjects = waterpumpProjects.Where(p => p.NameOfProject == projectName);
            }
            return resultList.Count != 0 ? resultList : waterpumpProjects;
        }
        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO)
        {            
            if(await GetWaterpumpProjectByName(waterpumpProjectDTO.NameOfProject) == null)
            {
                //compare dates.
                if(DateTime.Compare(waterpumpProjectDTO.StartDate,waterpumpProjectDTO.EndDate) < 0)
                {
                    WaterpumpProject wp = new WaterpumpProject()
                    {
                        ProjectId = Guid.NewGuid(),
                        Description = waterpumpProjectDTO.Description,
                        NameOfProject = waterpumpProjectDTO.NameOfProject /*!= null ? waterpumpProjectDTO.NameOfProject : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.NameOfProject)} provided")*/,
                        RatedPower = waterpumpProjectDTO.RatedPower,
                        FlowRate = waterpumpProjectDTO.FlowRate,
                        Coordinates = waterpumpProjectDTO.Coordinates,
                        CurrentTotal = 0,
                        TargetGoal = waterpumpProjectDTO.TargetGoal,
                        StartDate = waterpumpProjectDTO.StartDate /*!= default(DateTime) ? waterpumpProjectDTO.StartDate : throw new InvalidOperationException($"Invalid {nameof(waterpumpProjectDTO.StartDate)} provided.")*/,
                        EndDate = waterpumpProjectDTO.EndDate /*!= default(DateTime) ? waterpumpProjectDTO.EndDate : throw new InvalidOperationException($"Invalid {nameof(waterpumpProjectDTO.EndDate)} provided.")*/,
                        ProjectType = waterpumpProjectDTO.ProjectType,
                        PartitionKey = waterpumpProjectDTO.ProjectType.ToString() /*?? throw new ArgumentNullException($"Invalid value provided")*/
                    };
                    return await _waterpumpProjectWriteRepository.AddAsync(wp);
                }
                else
                {
                    throw new Exception("The start date provide much be no later than the end date provided.");
                }
            }
            else
            {
                throw new Exception("Project already exist");
            }
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumProject)
        {
            return await _waterpumpProjectWriteRepository.Update(waterpumProject);
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumProject, string projectId)
        {
            if(await GetWaterPumpProjectById(projectId) == null)
            {
                throw new InvalidOperationException("The project ID provided does not exist.");
            }
            return await _waterpumpProjectWriteRepository.Update(waterpumProject);
        }

        public async Task DeleteWaterpumpProjectAsync(string projectId)
        {
            WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
            await _waterpumpProjectWriteRepository.Delete(waterPumpProject);
        }
    }
}
