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
    public class IntegrationTestUser
    {
        private HttpClient _httpClient { get; }
        private string _token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImJiNzU5ZDFjLTFiM2YtNDlmMy1iNGYxLWY3OTE5MTAyYTZmZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RFIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYjc1OWQxYy0xYjNmLTQ5ZjMtYjRmMS1mNzkxOTEwMmE2ZmQiLCJuYmYiOjE2MzQ1NzAzNzUsImV4cCI6MTY4NjQxMDM3NSwiaWF0IjoxNjM0NTcwMzc1LCJpc3MiOiJEZWJ1Z0lzc3VlciIsImF1ZCI6IkRlYnVnQXVkaWVuY2UifQ.nvVcS52-ntRh1NwiBrMzLNYo6aLVhDSYPkf6fEBGOFA";

        private string _testUser;
        public IntegrationTestUser()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        #region Succesful Tests
        [Fact]
        public void GetAllUserSuccess()
        {
            HttpResponseMessage responseResult = _httpClient.GetAsync("api/users").Result;

            //responses
            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            //check results
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsType<List<User>>(users);
        }

        [Fact]
        public void FilterUserByFirstNameSuccess()
        {
            string firstName = "stephen";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?firstName={firstName}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            users.ForEach(
                s => Assert.Matches(firstName, s.FirstName));

        }

        [Fact]
        public void FilterUserByLastNameSuccess()
        {
            string lastName = "pangga";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?lasttName={lastName}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            users.ForEach(
                s => Assert.Matches(lastName, s.LastName));
        }

        [Fact]
        public void FilterUserBySubscriptionSuccess()
        {
            string subscription = "False";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?subscription={subscription}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            users.ForEach(
                s => Assert.Matches(subscription, s.Subscription.ToString()));
        }

        [Fact]
        public void DoCreateAndGetByIdAndEditAndDeleteUserSuccess()
        {
            CreateUserSuccess();
            GetUserbyIdSuccess();
            EditUserSuccess();
            DeleteUserSuccess();
        }

        public void GetUserbyIdSuccess()
        {
            string userId = _testUser;

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users/{userId}").Result;
            
            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(dataList);

            //check result
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.Matches(userId, user.UserId.ToString());
        }
        public void CreateUserSuccess()
        {
            UserDTO newUser = new UserDTO("John", "Doe", "John@email.com", "john123", false);


            HttpContent userData = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            //request
            HttpResponseMessage response = _httpClient.PostAsync($"api/users", userData).Result;

            var responseUserData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(responseUserData);

            //results
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<User>(user);

            //add the user id
            _testUser = user.UserId.ToString();
        }
        public void EditUserSuccess()
        {
            string userId = _testUser;

            UserDTO editUser = new UserDTO("John", "DoeThings", "John@email.com", "strongerPassword", true);

            HttpContent updatedUserData = new StringContent(JsonConvert.SerializeObject(editUser), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync($"api/users/{userId}", updatedUserData).Result;

            //process request
            var responseUserData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(responseUserData);

            //validate response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<User>(user);


        }
        public void DeleteUserSuccess()
        {
            string userId = _testUser;

            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/users/{userId}").Result;

            Assert.Equal(HttpStatusCode.Accepted, responseMessage.StatusCode);
        }
        #endregion



        #region Failed Tests
        [Fact]
        public void FilterUserByFirstNameFailure()
        {
            string firstName = "abc";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?firstName={firstName}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //users.ForEach(s => Assert.Matches(firstName, s.FirstName));
        }

        [Fact]
        public void FilterUserByLastNameFailure()
        {
            string lastName = "cba";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?lasttName={lastName}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //users.ForEach(s => Assert.Matches(lastName, s.LastName));
        }

        [Fact]
        public void FilterUserBySubscriptionFailure()
        {
            string subscription = "Falses";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?subscription={subscription}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //users.ForEach(s => Assert.Matches(subscription, s.Subscription.ToString()));
        }

        [Fact]
        public void GetUserByIdFailure()
        {
            string userId = "297cddd6-2935-4508-99cf-5da0d373d401";
            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users/{userId}").Result;
            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var user = JsonConvert.DeserializeObject<User>(dataList);
            //check result
            Assert.Equal(HttpStatusCode.BadRequest, responseResult.StatusCode);
            //Assert.Matches(userId, user.UserId.ToString());
        }

        [Fact]
        public void CreateUserFailure()
        {
            UserDTO newUser = new UserDTO("John", "Doe", "admin@email.com", "john123", false);

            HttpContent userData = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            //request
            HttpResponseMessage response = _httpClient.PostAsync($"api/users", userData).Result;

            var responseUserData = response.Content.ReadAsStringAsync().Result;
            //var user = JsonConvert.DeserializeObject<User>(responseUserData);

            //results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.IsType<User>(user);
        }

        [Fact]
        public void EditUserFailure()
        {
            string userId = "c598a49f-dd3e-43ec-a2a2-fd19d9873751";

            UserDTO editUser = new UserDTO("John", "DoeThings", "admin@email.com", "strongerPassword", true);

            HttpContent updatedUserData = new StringContent(JsonConvert.SerializeObject(editUser), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync($"api/users/{userId}", updatedUserData).Result;

            //process request
            var responseUserData = response.Content.ReadAsStringAsync().Result;
            //var user = JsonConvert.DeserializeObject<User>(responseUserData);

            //validate response
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.IsType<User>(user);


        }

        [Fact]
        public void DeleteUserFailure()
        {
            string userId = "8c0379a4-8750-4221-a64e-f677da5efb64";

            HttpResponseMessage responseMessage = _httpClient.DeleteAsync($"api/users/{userId}").Result;

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }
        #endregion
    }
}
