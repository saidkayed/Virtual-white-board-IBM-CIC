
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Whiteboard_API.Controllers;
using Whiteboard_API.Data;
using Whiteboard_API.Interfaces;
using Whiteboard_API.Model;


namespace Whiteboard_API.Tests
{
    public class AuthControllerTests
    {

        [Fact]
        async Task GetAllUsers_Returns_The_Correct_Number_Of_Users()
        {
            int count = 10;
            var users = Enumerable.Range(1, count).Select(i => new User { UserId = i, Username = $"User{i}" }).ToList();
            var mockUserRepository = A.Fake<IUserRepository>();
            A.CallTo(() => mockUserRepository.GetAllUsers()).Returns(Task.FromResult(users.AsEnumerable()));
            var authController = new AuthController(mockUserRepository);

            var result = await authController.GetAllUsers();
            var returnUsers = result.Value;
            Assert.Equal(count, returnUsers.Count());


            Assert.Equal(count, 10);


        }

        //create test if user can be created
        [Fact]
        async Task CreateUser_Returns_Created_User()
        {
            var mockUserRepository = A.Fake<IUserRepository>();
            var user = new User { UserId = 0, Username = "test" };
            A.CallTo(() => mockUserRepository.CreateUser(user)).Returns(Task.FromResult(user));
            var authController = new AuthController(mockUserRepository);

            var u = new UserDTO { Username = "test" };
            var actionresult = await authController.Register(u);
            var result = actionresult.Result as OkObjectResult;
            var returnUser = result.Value as User;
            Assert.Equal(user, returnUser);
        }

    }
}