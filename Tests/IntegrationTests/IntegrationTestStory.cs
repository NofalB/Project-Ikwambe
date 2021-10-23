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
        private string _token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImJiNzU5ZDFjLTFiM2YtNDlmMy1iNGYxLWY3OTE5MTAyYTZmZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RFIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYjc1OWQxYy0xYjNmLTQ5ZjMtYjRmMS1mNzkxOTEwMmE2ZmQiLCJuYmYiOjE2MzQ1NzAzNzUsImV4cCI6MTY4NjQxMDM3NSwiaWF0IjoxNjM0NTcwMzc1LCJpc3MiOiJEZWJ1Z0lzc3VlciIsImF1ZCI6IkRlYnVnQXVkaWVuY2UifQ.nvVcS52-ntRh1NwiBrMzLNYo6aLVhDSYPkf6fEBGOFA";


        public IntegrationTestStory()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");
            if (hostname == null)
                hostname = $"https://stichtingikwambe.azurewebsites.net/";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
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
            DateTime publishDate = DateTime.Parse("2021-10-22T11:31:14.343Z");

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={publishDate}").Result;

            var resultByPublishDate = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            var storyByPublishDate = JsonConvert.DeserializeObject<List<Story>>(resultByPublishDate);
            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            
            storyByPublishDate.ForEach(s => Assert.Matches(publishDate.ToString(), s.PublishDate.ToString()));
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
        public void CreateNewStorySuccess()
        {
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


        }

        [Fact]
        public void EditStorySuccess()
        {
            string storyId = "0c7e3765-84ba-494d-9607-3edec53d767b";

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

        [Fact]
        public void DeleteStorySuccess()
        {
            string storyId = "0c7e3765-84ba-494d-9607-3edec53d767b";
            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/stories/{storyId}").Result;
            Assert.Equal(HttpStatusCode.Accepted, responseMessage.StatusCode);
        }

        public void UploadImageToAnExistStorySuccess()
        {

        }

        #endregion

        #region Failed Test

        [Fact]
        public void FilterStoryByAuthorFailure()
        {
            string author = "string";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={author}").Result;

            var resultByAuthor = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            //var storyByAuthor = JsonConvert.DeserializeObject<List<Story>>(resultByAuthor);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            //storyByAuthor.ForEach(s => Assert.Matches(author, s.Author.ToString()));
        }

        [Fact]
        public void FilterByPublishDateFailure()
        {
            DateTime publishDate = DateTime.Parse("2021-12-21");

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={publishDate}").Result;

            var resultByPublishDate = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            //var storyByPublishDate = JsonConvert.DeserializeObject<List<Story>>(resultByPublishDate);
            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);

            //storyByPublishDate.ForEach(s => Assert.Matches(publishDate, s.PublishDate.ToString()));
        }

        [Fact]
        public void GetStoryByIdFailure()
        {
            string storyId = "fbf38ffa-d7f6-477b-9400-51182ad14372";
            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/stories/{storyId}").Result;
            //response
            var responseData = responseResult.Content.ReadAsStringAsync().Result;
            //var story = JsonConvert.DeserializeObject<Story>(responseData);
            //check result
            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //Assert.Matches(storyId, story.StoryId.ToString());
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
            //var story = JsonConvert.DeserializeObject<Story>(storyResponseData);

            //validate the resul
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.IsType<Story>(story);


        }

        [Fact]
        public void EditStoryFailure()
        {
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
            //var story = JsonConvert.DeserializeObject<Story>(storyResponseData);

            //validate the resul
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.IsType<Story>(story);
        }

        [Fact]
        public void DeleteStoryFailure()
        {
            string storyId = "0c7e3765-84ba-494d-9607-3edec53d7671";
            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/stories/{storyId}").Result;
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        public void UploadImageToAnExistStoryFailure()
        {

        }
        #endregion
    }
}
