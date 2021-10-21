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
                hostname = $"http://localhost:{7071}";
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

        public void FilterStoryByAuthorSuccess()
        {
            string author = "";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={author}").Result;

            var resultByAuthor = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            var storyByAuthor = JsonConvert.DeserializeObject<List<Story>>(resultByAuthor);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            storyByAuthor.ForEach(s => Assert.Matches(author, s.Author));
        }

        public void FilterByPublishDateSuccess()
        {
            string publishDate = "";

            HttpResponseMessage responseWithStoryAuthor = _httpClient.GetAsync($"api/stories?author={publishDate}").Result;

            var resultByPublishDate = responseWithStoryAuthor.Content.ReadAsStringAsync().Result;
            var storyByPublishDate = JsonConvert.DeserializeObject<List<Story>>(resultByPublishDate);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseWithStoryAuthor.StatusCode);
            storyByPublishDate.ForEach(s => Assert.Matches(publishDate, s.PublishDate.ToString()));
        }

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


        }

        public void EditStorySuccess()
        {

        }

        public void DeleteStorySuccess()
        {

        }

        public void UploadImageToAnExistStorySuccess()
        {

        }

        #endregion

        #region Failed Test


        #endregion
    }
}
