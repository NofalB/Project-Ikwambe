using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Headers;
using Domain;
using Domain.DTO;
using Newtonsoft.Json;
using System.Net;

namespace IntegrationTests
{
    public class IntegrationTestWaterpumpProject
    {
        private HttpClient _httpClient { get; }
        private string _token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImJiNzU5ZDFjLTFiM2YtNDlmMy1iNGYxLWY3OTE5MTAyYTZmZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RFIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYjc1OWQxYy0xYjNmLTQ5ZjMtYjRmMS1mNzkxOTEwMmE2ZmQiLCJuYmYiOjE2MzQ1NzAzNzUsImV4cCI6MTY4NjQxMDM3NSwiaWF0IjoxNjM0NTcwMzc1LCJpc3MiOiJEZWJ1Z0lzc3VlciIsImF1ZCI6IkRlYnVnQXVkaWVuY2UifQ.nvVcS52-ntRh1NwiBrMzLNYo6aLVhDSYPkf6fEBGOFA";

        //this string will be used for deleting and edit test.
        private string _createdProjectId;

        public IntegrationTestWaterpumpProject()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }


        #region Succesful tests

        [Fact]
        public void DoCreateAndGetByIdAndEditAndDeleteSuccess()
        {
            CreateWaterpumpProjectSuccess();
            FilterWaterpumpProjectByProjectTypeSuccess();
            FilterWaterpumpProjectByProjectNameSuccess();
            GetAWaterpumpProjectByIdSuccess();
            EditWaterpumpProjectSuccess();
            DeleteWaterpumpProjectSuccess();
        }

        public void GetAllWaterpumpProjectSuccess()
        {
            HttpResponseMessage responseResult = _httpClient.GetAsync("api/waterpumps").Result;

            //responses
            var data = responseResult.Content.ReadAsStringAsync().Result;
            var waterpumpProjects = JsonConvert.DeserializeObject<List<WaterpumpProject>>(data);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsType<List<WaterpumpProject>>(waterpumpProjects);
        }
        public void FilterWaterpumpProjectByProjectTypeSuccess()
        {
            string projectType = "health_service";

            //request
            HttpResponseMessage responseWithProjectType = _httpClient.GetAsync($"api/waterpumps?projecttype={projectType}").Result;

            //get response
            var resultProjectByProjectType = responseWithProjectType.Content.ReadAsStringAsync().Result;
            var waterpumpProjectByType = JsonConvert.DeserializeObject<List<WaterpumpProject>>(resultProjectByProjectType);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectType.StatusCode);
            waterpumpProjectByType.ForEach(
                w => Assert.Matches(projectType, w.ProjectType.ToString()
                ));
        }
        public void FilterWaterpumpProjectByProjectNameSuccess()
        {
            string projectName = "Project Waterpump"; //this need to be changed later

            //request
            HttpResponseMessage responseWithProjectName = _httpClient.GetAsync($"api/waterpumps?projectName={projectName}").Result;

            //get response
            var resultProjectByName = responseWithProjectName.Content.ReadAsStringAsync().Result;
            var waterpumpProjectByName = JsonConvert.DeserializeObject<List<WaterpumpProject>>(resultProjectByName);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectName.StatusCode);
            waterpumpProjectByName.ForEach(
                w => Assert.Matches(projectName, w.NameOfProject));
        }
        public void GetAWaterpumpProjectByIdSuccess()
        {
            string projectId = _createdProjectId;

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/waterpumps/{projectId}").Result;

            //response
            var responseData = responseResult.Content.ReadAsStringAsync().Result;
            var waterpumpProject = JsonConvert.DeserializeObject<WaterpumpProject>(responseData);

            //check result
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.Matches(projectId, waterpumpProject.ProjectId.ToString());
        }
        public void CreateWaterpumpProjectSuccess()
        {
            WaterpumpProjectDTO TestProjectData = new WaterpumpProjectDTO()
            {
                NameOfProject = "Test Project",
                Description = "Test Description",
                Coordinates = new Coordinates("Test", -8.00, 36.833330),
                TargetGoal = 100000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                RatedPower = 1200,
                FlowRate = 1600,
                ProjectType = ProjectType.health_service
            };

            HttpContent waterpumpProjectData = new StringContent(JsonConvert.SerializeObject(TestProjectData), Encoding.UTF8, "application/json");
            // make a request
            HttpResponseMessage responseMessage = _httpClient.PostAsync($"api/waterpumps", waterpumpProjectData).Result;
            //process the request
            var responseWaterpumpData = responseMessage.Content.ReadAsStringAsync().Result;
            var waterpump = JsonConvert.DeserializeObject<WaterpumpProject>(responseWaterpumpData);
            //check results
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.IsType<WaterpumpProject>(waterpump);
            //add the Id of the created project to this string.
            _createdProjectId = waterpump.ProjectId.ToString();
        }
        public void EditWaterpumpProjectSuccess()
        {
            string projectId = _createdProjectId;

            WaterpumpProject TestProjectData = new WaterpumpProject()
            {
                NameOfProject = "Update Test Project1111",
                Description = "Test Description has been updated",
                Coordinates = new Coordinates("Test", -8.00, 36.833330),
                CurrentTotal = 0,
                TargetGoal = 100000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                RatedPower = 1200,
                FlowRate = 16001212,
                ProjectType = ProjectType.health_service,
                PartitionKey = ProjectType.health_service.ToString()
            };

            HttpContent waterpumpProjectData = new StringContent(JsonConvert.SerializeObject(TestProjectData), Encoding.UTF8, "application/json");

            // make a request
            HttpResponseMessage responseMessage = _httpClient.PutAsync($"api/waterpumps/{projectId}", waterpumpProjectData).Result;

            //process the request
            var responseWaterpumpData = responseMessage.Content.ReadAsStringAsync().Result;
            var waterpump = JsonConvert.DeserializeObject<WaterpumpProject>(responseWaterpumpData);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.IsType<WaterpumpProject>(waterpump);
        }
        public void DeleteWaterpumpProjectSuccess()
        {
            string projectId = _createdProjectId;

            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/waterpumps/{projectId}").Result;

            Assert.Equal(HttpStatusCode.Accepted, responseMessage.StatusCode);
        }
        #endregion


        #region Failed tests
        [Fact]
        public void FilterWaterpumpProjectByProjectTypeFailed()
        {
            string projectType = "23452345";

            //request
            HttpResponseMessage responseWithProjectType = _httpClient.GetAsync($"api/waterpumps?projecttype={projectType}").Result;

            //get response
            var resultProjectByProjectType = responseWithProjectType.Content.ReadAsStringAsync().Result;
            //var waterpumpProjectByType = JsonConvert.DeserializeObject<WaterpumpProject>(resultProjectByProjectType);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectType.StatusCode);
            //Assert.Empty((System.Collections.IEnumerable)waterpumpProjectByType);
        }
        [Fact]
        public void FilterWaterpumpProjectByProjectNameFailure()
        {
            string projectName = "absss"; //this need to be changed later

            //request
            HttpResponseMessage responseWithProjectName = _httpClient.GetAsync($"api/waterpumps?projectName={projectName}").Result;

            //get response
            var resultProjectByName = responseWithProjectName.Content.ReadAsStringAsync().Result;
            var waterpumpProjectByName = JsonConvert.DeserializeObject<List<WaterpumpProject>>(resultProjectByName).ToList();

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectName.StatusCode);
            //Assert.Empty(waterpumpProjectByName);
        }
        [Fact]
        public void GetWaterpumpProjectByIdFailure()
        {
            string projectId = "9ac7ee43-f464-46ca-85b9-b1549a258e51";
            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/waterpumps/{projectId}").Result;
            //response
            var responseData = responseResult.Content.ReadAsStringAsync().Result;
            //var waterpumpProject = JsonConvert.DeserializeObject<WaterpumpProject>(responseData);
            //check result
            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //Assert.Null(waterpumpProject);
        }
        [Fact]
        public void CreateWaterpumpProjectFailure()
        {
            WaterpumpProjectDTO TestProjectData = new WaterpumpProjectDTO()
            {
                NameOfProject = "Test Project",
                Description = "Test Description",
                Coordinates = new Coordinates("Test", -8.00, 036.833330),
                TargetGoal = 100000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-1),
                FlowRate = 1600,
                ProjectType = ProjectType.health_service
            };

            HttpContent waterpumpProjectData = new StringContent(JsonConvert.SerializeObject(TestProjectData), Encoding.UTF8, "application/json");
            // make a request
            HttpResponseMessage responseMessage = _httpClient.PostAsync($"api/waterpumps", waterpumpProjectData).Result;
            //process the request
            var responseWaterpumpData = responseMessage.Content.ReadAsStringAsync().Result;
            //var waterpump = JsonConvert.DeserializeObject<WaterpumpProject>(responseWaterpumpData);
            //check results
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            //Assert.IsType<WaterpumpProject>(waterpump);
        }
        [Fact]
        public void EditWaterpumpProjectFailure()
        {
            string projectId = "9ac7ee43-f464-46ca-85b9-b1549a258e51";

            WaterpumpProjectDTO TestProjectData = new WaterpumpProjectDTO()
            {
                NameOfProject = "Update Test Project",
                Description = "Test Description has been updated",
                Coordinates = new Coordinates("Test", -8.00, 36.833330),
                TargetGoal = 100000,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now,
                RatedPower = 1200,
                ProjectType = ProjectType.health_service
            };

            HttpContent waterpumpProjectData = new StringContent(JsonConvert.SerializeObject(TestProjectData), Encoding.UTF8, "application/json");

            // make a request
            HttpResponseMessage responseMessage = _httpClient.PutAsync($"api/waterpumps/{projectId}", waterpumpProjectData).Result;

            //process the request
            var responseWaterpumpData = responseMessage.Content.ReadAsStringAsync().Result;
            //var waterpump = JsonConvert.DeserializeObject<WaterpumpProject>(responseWaterpumpData);

            //check results
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            //Assert.IsType<WaterpumpProject>(waterpump);
        }
        [Fact]
        public void DeleteWaterpumpProjectFailure()
        {
            string projectId = "8c0379a4-8750-4221-a64e-f677da5efb64";

            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/waterpumps/{projectId}").Result;

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }
        #endregion
    }
}
