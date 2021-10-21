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
            try
            {
                Guid id = !string.IsNullOrEmpty(projectId) ? Guid.Parse(projectId) : throw new ArgumentNullException("No project ID was provided.");
                var project = await _waterpumpProjectReadRepository.GetAll().FirstOrDefaultAsync(w => w.ProjectId == id);

                if (project == null)
                {
                    throw new InvalidOperationException($"No project exists with the ID {projectId}");
                }
                return project;
            }
            catch
            {
                throw new InvalidOperationException("Invalid project ID provided.");
            }
        }

        public async Task<WaterpumpProject> GetWaterpumpProjectByName(string projectName)
        {
            projectName = !string.IsNullOrEmpty(projectName) ? projectName : throw new ArgumentNullException("No project Name was provided.");
            WaterpumpProject projectData = await _waterpumpProjectReadRepository.GetAll().FirstOrDefaultAsync(w => w.NameOfProject == projectName);
            if (projectData != null)
            {
                throw new ArgumentNullException("No waterpump project found with the provided name");
            }
            return projectData;
        }

        public List<WaterpumpProject> GetWaterpumpProjectByQuery(string projectType, string projectName)
        {
            List<WaterpumpProject> resultList = _waterpumpProjectReadRepository.GetAll().ToList();

            if (!string.IsNullOrEmpty(projectType))
            {
                ProjectType pt = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);

                resultList = resultList.Where(p =>
                {
                    try
                    {
                        return p.ProjectType == pt;
                    }
                    catch
                    {
                        throw new InvalidOperationException("Invalid ProjectType Provided");
                    }
                }).ToList();
            }
            if (!string.IsNullOrEmpty(projectName))
            {
                resultList  = resultList.Where(p =>
                {
                    try
                    {
                        return p.NameOfProject == projectName;
                    }
                    catch
                    {
                        throw new InvalidOperationException("The Project name you have provided is invalid or does not exist");

                    }
                    
                }).ToList();
            }

            return resultList.Count != 0 ? resultList : new List<WaterpumpProject>();
        }
        public async Task<WaterpumpProject> AddWaterpumpProject(WaterpumpProjectDTO waterpumpProjectDTO)
        {
            if (waterpumpProjectDTO == null)
            {
                throw new NullReferenceException($"{nameof(waterpumpProjectDTO)} cannot be null.");
            }

            if (await GetWaterpumpProjectByName(waterpumpProjectDTO.NameOfProject) == null)
            {
                waterpumpProjectDTO.StartDate = waterpumpProjectDTO.StartDate != default ? waterpumpProjectDTO.StartDate : throw new InvalidOperationException($"Start date is null.");
                waterpumpProjectDTO.EndDate = waterpumpProjectDTO.EndDate != default ? waterpumpProjectDTO.EndDate : throw new InvalidOperationException($"End date is null.");

                //compare dates.
                if (DateTime.Compare(waterpumpProjectDTO.StartDate,waterpumpProjectDTO.EndDate) < 0)
                {
                    WaterpumpProject wp = new WaterpumpProject()
                    {
                        ProjectId = Guid.NewGuid(),
                        Description = !string.IsNullOrEmpty(waterpumpProjectDTO.Description) ? waterpumpProjectDTO.Description : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.Description)} provided"),
                        NameOfProject = !string.IsNullOrEmpty(waterpumpProjectDTO.NameOfProject) ? waterpumpProjectDTO.NameOfProject : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.NameOfProject)} provided"),
                        RatedPower = waterpumpProjectDTO.RatedPower != 0 ? waterpumpProjectDTO.RatedPower : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.RatedPower)} provided"),
                        FlowRate = waterpumpProjectDTO.FlowRate != 0 ? waterpumpProjectDTO.FlowRate : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.FlowRate)} provided"),
                        Coordinates = waterpumpProjectDTO.Coordinates ?? throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.Coordinates)} provided"),
                        CurrentTotal = 0,
                        TargetGoal = waterpumpProjectDTO.TargetGoal != 0 ? waterpumpProjectDTO.TargetGoal : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.TargetGoal)} provided"),
                        StartDate = waterpumpProjectDTO.StartDate,
                        EndDate = waterpumpProjectDTO.EndDate,
                        ProjectType = waterpumpProjectDTO.ProjectType,
                        PartitionKey = waterpumpProjectDTO.ProjectType.ToString()
                    };
                    return await _waterpumpProjectWriteRepository.AddAsync(wp);
                }
                else
                {
                    throw new InvalidOperationException("The end date should be no later than start date.");
                }
            }
            else
            {
                throw new InvalidOperationException("Project already exist");
            }
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProject waterpumProject)
        {
            return await _waterpumpProjectWriteRepository.Update(waterpumProject);
        }

        public async Task<WaterpumpProject> UpdateWaterPumpProject(WaterpumpProjectDTO waterpumpProjectDTO, string projectId)
        {
            WaterpumpProject project = await GetWaterPumpProjectById(projectId);
            if(project != null)
            {
                if (DateTime.Compare(waterpumpProjectDTO.StartDate, waterpumpProjectDTO.EndDate) < 0)
                {
                    project.Description = !string.IsNullOrEmpty(waterpumpProjectDTO.Description) ? waterpumpProjectDTO.Description : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.Description)} provided");
                    project.NameOfProject = !string.IsNullOrEmpty(waterpumpProjectDTO.NameOfProject) ? waterpumpProjectDTO.NameOfProject : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.NameOfProject)} provided");
                    project.RatedPower = waterpumpProjectDTO.RatedPower != 0 ? waterpumpProjectDTO.RatedPower : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.RatedPower)} provided");
                    project.FlowRate = waterpumpProjectDTO.FlowRate != 0 ? waterpumpProjectDTO.FlowRate : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.FlowRate)} provided");
                    project.Coordinates = waterpumpProjectDTO.Coordinates ?? throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.Coordinates)} provided");
                    project.TargetGoal = waterpumpProjectDTO.TargetGoal != 0 ? waterpumpProjectDTO.TargetGoal : throw new ArgumentException($"Invalid {nameof(waterpumpProjectDTO.TargetGoal)} provided");
                    project.StartDate = waterpumpProjectDTO.StartDate;
                    project.EndDate = waterpumpProjectDTO.EndDate;
                    project.ProjectType = waterpumpProjectDTO.ProjectType;

                    return await _waterpumpProjectWriteRepository.Update(project);
                }
                throw new InvalidOperationException("The start date provide much be no later than the end date provided.");
            }
            throw new InvalidOperationException("The project ID provided does not exist.");

        }

        public async Task DeleteWaterpumpProjectAsync(string projectId)
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                WaterpumpProject waterPumpProject = await GetWaterPumpProjectById(projectId);
                await _waterpumpProjectWriteRepository.Delete(waterPumpProject);
            }
            else
            {
                throw new ArgumentNullException("Project ID does not exist");
            }
        }
    }
}
