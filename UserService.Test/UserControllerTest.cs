using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService;
using UserService.Test;
using UserService.Interfaces;
using UserService.Models;
using UserService.Controllers;

namespace UserService.Test
{
    [TestFixture]
    public class UserControllerTest
    {
        private UserController userController;
        private Mock<IUser> _userService;
        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUser>();
            userController = new UserController(_userService.Object);
        }
        [Test]
        public async Task GeUserById_ProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var userId = 1;
            var expectedProduct = new ViewUserModel { 
                Id = userId, FirstName = "Vinod",LastName="Kumar",EmailId="vinod@yopmail.com",
                PhoneNum = "986544333",Role="Admin"
            };

            _userService
                .Setup(service => service.GetUserById(userId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await userController.GetUserById(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedProduct, okResult.Value);
        }
    }
}
