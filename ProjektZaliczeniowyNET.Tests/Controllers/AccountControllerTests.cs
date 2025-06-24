// ProjektZaliczeniowyNET.Tests/Controllers/AccountControllerTests.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ProjektZaliczeniowyNET.Controllers;
using ProjektZaliczeniowyNET.Models;
using System.Collections.Generic;

namespace ProjektZaliczeniowyNET.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void Register_GET_ReturnsView()
        {
            // Arrange
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager(userManager);
            var emailSender = new Mock<IEmailSender>();

            var controller = new AccountController(
                userManager.Object, 
                signInManager.Object, 
                emailSender.Object);

            // Act
            var result = controller.Register();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        private Mock<UserManager<ApplicationUser>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            
            // Dodaj setup dla ApplicationUser
            store.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<System.Threading.CancellationToken>()))
                 .ReturnsAsync(IdentityResult.Success);

            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
            
            return userManager;
        }

        private Mock<SignInManager<ApplicationUser>> CreateMockSignInManager(Mock<UserManager<ApplicationUser>> userManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            var signInManager = new Mock<SignInManager<ApplicationUser>>(
                userManager.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);
            
            return signInManager;
        }
    }
}
