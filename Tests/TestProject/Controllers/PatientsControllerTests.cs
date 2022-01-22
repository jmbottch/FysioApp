﻿using ApplicationCore.Abstractions;
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
using System.Threading.Tasks;
using Xunit;

namespace TestProject.Controllers
{
    public class PatientsControllerTests
    {
        [Fact]
        public async void Create_Patient_Creates_When_Patient_Is_Older_Than_Sixteen()
        {
            var helper = new Testhelper();

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

            var patient = helper.CreateTestPatient();
            RegisterPatientViewModel model = new RegisterPatientViewModel()
            {
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                AvansNumber = patient.AvansNumber,
                AvansRole = patient.AvansRole,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
            };
            var patientRepo = helper.GetInMemoryPatientRepo();

            var sut = new PatientsController(new Mock<IIdentityUserRepository>().Object, patientRepo);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user};

            await sut.Create(model);

            var result = patientRepo.GetPatients().ToList().Count();

            Assert.Equal(1, result);

        }

        [Fact]
        public async void Create_Patient_Returns_Error_When_Patient_Is_Younger_Than_Sixteen()
        {
            var helper = new Testhelper();

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

            var patient = helper.CreateTestPatient();
            RegisterPatientViewModel model = new RegisterPatientViewModel()
            {
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                AvansNumber = patient.AvansNumber,
                AvansRole = patient.AvansRole,
                Gender = patient.Gender,
                DateOfBirth = DateTime.Now.AddYears(-10)
            };
            var patientRepo = helper.GetInMemoryPatientRepo();

            var sut = new PatientsController(new Mock<IIdentityUserRepository>().Object, patientRepo);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            await sut.Create(model);

            var result = patientRepo.GetPatients().ToList().Count();

            Assert.Equal(0, result);
        }
    }
}
