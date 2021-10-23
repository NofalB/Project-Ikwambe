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
    public class IntegrationTestStory
    {
        private HttpClient _httpClient { get; }
        private string _userToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsIm5iZiI6MTYzNDg1MzA0MiwiZXhwIjoxNjg2NjkzMDQyLCJpYXQiOjE2MzQ4NTMwNDIsImlzcyI6IkRlYnVnSXNzdWVyIiwiYXVkIjoiRGVidWdBdWRpZW5jZSJ9.XetnRbFBJwhvOJQauam80MF1t8hqxhXurBT3s7G0zJA";
        private string _adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiIzNjJiZjIxMi1hNWJjLTQ5ZTQtOTRlYi01ZTVkM2ExZmJmODYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMzYyYmYyMTItYTViYy00OWU0LTk0ZWItNWU1ZDNhMWZiZjg2IiwibmJmIjoxNjM0ODUzMDEwLCJleHAiOjE2ODY2OTMwMTAsImlhdCI6MTYzNDg1MzAxMCwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.afTG3OzeVVOkRaMuNXXtQqpUu5OcoQQD3UmyrjFvPAk";

        //create token Id
        private string story_Id;

        public IntegrationTestStory()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");
            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        }

        #region Successful Tests
        [Fact]
        public void GetAllStorySuccess()
        {
            HttpResponseMessage responseResult = _httpClient.GetAsync("api/stories").Result;

            //responses
            var data = responseResult.Content.ReadAsStringAsync().Result;
            var stories = JsonConvert.DeserializeObject<List<Story>>(data);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsType<List<Story>>(stories);
        }

        [Fact]
        public void FilterStoryByAuthorSuccess()
        {
            string author = "string";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={author}").Result;

            var resultByAuthor = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            var storyByAuthor = JsonConvert.DeserializeObject<List<Story>>(resultByAuthor);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            storyByAuthor.ForEach(s => Assert.Matches(author, s.Author.ToString()));
        }

        [Fact]
        public void FilterByPublishDateSuccess()
        {
            string Date = "21/10/2021";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?publishDate=2021-10-21").Result;

            var resultByPublishDate = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            var storyByPublishDate = JsonConvert.DeserializeObject<List<Story>>(resultByPublishDate);
            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            storyByPublishDate.ForEach(s => Assert.Matches(Date.ToString(), s.PublishDate.ToString()));
        }

        [Fact]
        public void GetStoryByIdSuccess()
        {
            string storyId = "fbf38ffa-d7f6-477b-9400-51182ad14376";
            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/stories/{storyId}").Result;
            //response
            var responseData = responseResult.Content.ReadAsStringAsync().Result;
            var story = JsonConvert.DeserializeObject<Story>(responseData);
            //check result
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.Matches(storyId, story.StoryId.ToString());
        }

        [Fact]
        public void DoCreateEditAndDel()
        {
            CreateNewStorySuccess();
            EditStorySuccess();
            DeleteStorySuccess();
        }

        public void CreateNewStorySuccess()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            StoryDTO TestStoryData = new StoryDTO()
            {
                Title = "This is a test story",
                Summary = "This is the summary of the test story",
                Description = "This is Description of the test story",
                PublishDate = DateTime.Now,
                Author = "stephen",
                ImageURL = "Rubber Duckies"
            };

            HttpContent storyData = new StringContent(JsonConvert.SerializeObject(TestStoryData), Encoding.UTF8, "application/json");
            //call the request
            HttpResponseMessage response = _httpClient.PostAsync($"api/stories", storyData).Result;

            var storyResponseData = response.Content.ReadAsStringAsync().Result;
            var story = JsonConvert.DeserializeObject<Story>(storyResponseData);

            //validate the resul
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Story>(story);

            story_Id = story.StoryId.ToString();
        }

        public void EditStorySuccess()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            string storyId = story_Id;

            StoryDTO UpdateStoryData = new StoryDTO()
            {
                Title = "This is a update test story",
                Summary = "This is the summary of the test story",
                Description = "This is Description of the test story",
                PublishDate = DateTime.Now,
                Author = "stephen",
                ImageURL = "Rubber Duckies"
            };

            HttpContent storyUpdateData = new StringContent(JsonConvert.SerializeObject(UpdateStoryData), Encoding.UTF8, "application/json");
            //call the request
            HttpResponseMessage response = _httpClient.PutAsync($"api/stories/{storyId}", storyUpdateData).Result;

            var storyResponseData = response.Content.ReadAsStringAsync().Result;
            var story = JsonConvert.DeserializeObject<Story>(storyResponseData);

            //validate the resul
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Story>(story);
        }

        public void DeleteStorySuccess()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            string storyId = story_Id;
            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/stories/{storyId}").Result;
            Assert.Equal(HttpStatusCode.Accepted, responseMessage.StatusCode);
        }

        #endregion

        #region Failed Test

        [Fact]
        public void FilterStoryByAuthorFailure()
        {
            string author = "string";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={author}").Result;

            var resultByAuthor = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
        }

        [Fact]
        public void FilterByPublishDateFailure()
        {
            DateTime publishDate = DateTime.Parse("2021-12-21");

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={publishDate}").Result;

            var resultByPublishDate = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
        }

        [Fact]
        public void GetStoryByIdFailure()
        {
            string storyId = "fbf38ffa-d7f6-477b-9400-51182ad14372";
            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/stories/{storyId}").Result;
            //response
            var responseData = responseResult.Content.ReadAsStringAsync().Result;
            //check result
            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
        }

        [Fact]
        public void CreateNewStoryFailure()
        {
            StoryDTO TestStoryData = new StoryDTO()
            {
                Title = "=",
                Summary = 00000.ToString(),
                Description = "This is Description of the test story",
                PublishDate = DateTime.Now,
                Author = "",
                ImageURL = "Rubber Duckies"
            };

            HttpContent storyData = new StringContent(JsonConvert.SerializeObject(TestStoryData), Encoding.UTF8, "application/json");
            //call the request
            HttpResponseMessage response = _httpClient.PostAsync($"api/stories", storyData).Result;

            var storyResponseData = response.Content.ReadAsStringAsync().Result;

            //validate the resul
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }

        [Fact]
        public void EditStoryFailure()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            string storyId = "0c7e3765-84ba-494d-9607-3edec53d7671";

            StoryDTO UpdateStoryData = new StoryDTO()
            {
                Title = "This is a update test story",
                Summary = "This is the summary of the test story",
                Description = "This is Description of the test story",
                PublishDate = DateTime.Now,
                Author = "stephen",
                ImageURL = "Rubber Duckies"
            };

            HttpContent storyUpdateData = new StringContent(JsonConvert.SerializeObject(UpdateStoryData), Encoding.UTF8, "application/json");
            //call the request
            HttpResponseMessage response = _httpClient.PutAsync($"api/stories/{storyId}", storyUpdateData).Result;

            var storyResponseData = response.Content.ReadAsStringAsync().Result;

            //validate the resul
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void DeleteStoryFailure()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            string storyId = "0c7e3765-84ba-494d-9607-3edec53d7671";
            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/stories/{storyId}").Result;
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        #endregion
    }
}
