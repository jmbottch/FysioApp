using ApplicationCore.Abstractions;
using ApplicationCore.Utility;
using FysioApp.Controllers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.Controllers
{
    public class PatientsControllerTests
    {
        Testhelper helper = new Testhelper();

        [Fact]
        public async void Create_Patient_Returns_Error_When_Patient_Is_Younger_Than_Sixteen()
        {            

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Role, StaticDetails.StudentEndUser),
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ClaimTypes.NameIdentifier, "Id")
                    },
                    "TestAuthentication"
                    ));
            
            RegisterPatientViewModel model = new RegisterPatientViewModel()
            {
                Email = "testing@email.com",
                FirstName = "testing",
                LastName = "patient",
                PhoneNumber = "061234567",
                AvansNumber = 2114785,
                AvansRole = "1",
                Gender = "0",
                DateOfBirth = DateTime.Now.AddYears(-10)
            };
            var patientRepo = helper.GetInMemoryPatientRepo();

            var sut = new PatientsController(new Mock<IIdentityUserRepository>().Object, patientRepo);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            await sut.Create(model);

            var result = patientRepo.GetPatients().ToList().Count();

            Assert.Equal(1, result); //only the original testpatient from the testhelper will be in the db
        }
    }
}
