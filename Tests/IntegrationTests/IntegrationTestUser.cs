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
        private string _userToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsIm5iZiI6MTYzNDg1MzA0MiwiZXhwIjoxNjg2NjkzMDQyLCJpYXQiOjE2MzQ4NTMwNDIsImlzcyI6IkRlYnVnSXNzdWVyIiwiYXVkIjoiRGVidWdBdWRpZW5jZSJ9.XetnRbFBJwhvOJQauam80MF1t8hqxhXurBT3s7G0zJA";
        private string _adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiIzNjJiZjIxMi1hNWJjLTQ5ZTQtOTRlYi01ZTVkM2ExZmJmODYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMzYyYmYyMTItYTViYy00OWU0LTk0ZWItNWU1ZDNhMWZiZjg2IiwibmJmIjoxNjM0ODUzMDEwLCJleHAiOjE2ODY2OTMwMTAsImlhdCI6MTYzNDg1MzAxMCwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.afTG3OzeVVOkRaMuNXXtQqpUu5OcoQQD3UmyrjFvPAk";

        private string _testUser;
        public IntegrationTestUser()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:7071/";

                _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        }

        #region Succesful Tests
        [Fact]
        public void GetAllUserSuccess()
        {
            HttpResponseMessage responseResult = _httpClient.GetAsync("api/users").Result;

            //responses
            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var users = JsonConvert.DeserializeObject<List<dynamic>>(dataList);
            var users = JsonConvert.DeserializeObject<List<Object.UserTest>>(dataList);
            
            //check results
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsType<List<Object.UserTest>>(users);
            //Assert.IsType<List<UserResponseDTO>>(users);
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
            //string userId = "70b62b44-bbb6-4696-a60e-5d3cf9ce6746";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users/{userId}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<Object.UserTest>(dataList);

            //check result
            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            Assert.Matches(userId, user.UserId.ToString());
        }
        
        public void CreateUserSuccess()
        {
            UserDTO newUser = new UserDTO("John", "Doe", "John@gmail.com", "John@123!", false);

            HttpContent userData = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            //request
            HttpResponseMessage response = _httpClient.PostAsync($"api/users", userData).Result;

            var responseUserData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(responseUserData);

            //results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<User>(user);

            //add the user id
            _testUser = user.UserId.ToString();
        }
        
        public void EditUserSuccess()
        {
            string userId = _testUser;
            //string userId = "71379f8f-b2e1-4b2f-a19b-e12d6f7f63d1";

            UserDTO editUser = new UserDTO("John", "DoeThings", "John@gmail.com", "John@123!", true);

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

            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
            //users.ForEach(s => Assert.Matches(firstName, s.FirstName));
        }

        [Fact]
        public void FilterUserByLastNameFailure()
        {
            string lastName = "cba";

            HttpResponseMessage responseResult = _httpClient.GetAsync($"api/users?lasttName={lastName}").Result;

            var dataList = responseResult.Content.ReadAsStringAsync().Result;
            //var users = JsonConvert.DeserializeObject<List<User>>(dataList);

            Assert.Equal(HttpStatusCode.OK, responseResult.StatusCode);
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
            UserDTO newUser = new UserDTO("John", "Doe", "stephen@email.com", "john123", false);

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
