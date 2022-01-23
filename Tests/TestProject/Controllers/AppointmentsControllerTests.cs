using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using ApplicationCore.Utility;
using AutoFixture;
using AutoFixture.Xunit2;
using FysioApp.Controllers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using FysioApp.Models.ViewModels.AppointmentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject.Controllers
{
    public class AppointmentsControllerTests
    {
        Testhelper helper = new Testhelper();
        //if endtime > availability.Endtime
        [Fact]       
        public async void Create_Appointment_Should_Return_Error_When_Chosen_Time_Is_Too_Late_For_Student()
        {

            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();         

            //this will be the object the test will do checks on
            //if appointment.endtime > student.availability.endtime, error            
            Appointment appointment = new Appointment()
            {
                Id = 1,
                PatientId = "a",
                StudentId = "b",
                //DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime
                DateTime = new DateTime(2027, 01, 21, 16, 30, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 17, 30, 00),
                IsCancelled = false
            };

            CreateAppointmentViewModel model = new CreateAppointmentViewModel()
            {
                Appointment = appointment
            };         
            
            var appointmentRepo = helper.GetInMemoryAppointmentRepo();            
            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();   

            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);          

            await sut.Create(model);

            var result = appointmentRepo.GetAppointments().ToList().Count();           
           
            //assert no appointment has been added
            Assert.Equal(0, result);


        }

        //starttime < availability.starttime
        [Fact]
        public async void Create_Appointment_Should_Return_Error_When_Chosen_Time_Is_Too_Early_For_Student()
        {          

            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();           

            Appointment appointment = new Appointment()
            {
                Id = 2,
                PatientId = "a",
                StudentId = "b",
                //DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime
                DateTime = new DateTime(2027, 01, 21, 08, 30, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 09, 30, 00),
                IsCancelled = false
            };

            CreateAppointmentViewModel model = new CreateAppointmentViewModel()
            {
                Appointment = appointment
            };

            var appointmentRepo = helper.GetInMemoryAppointmentRepo();
            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();

            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);

            await sut.Create(model);

            var result = appointmentRepo.GetAppointments().ToList().Count();

            //assert no appointment has been added
            Assert.Equal(0, result);


        }

        //an apointment has already been made.
        [Fact]
        public async Task Create_Appointment_Should_Return_Error_When_Student_Already_Has_Appointment()
        {
            

            //set up required entities

            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();            

            Appointment appointment = new Appointment()
            {
                Id = 3,
                PatientId = "a",
                StudentId = "b",
                DateTime = new DateTime(2027, 01, 21, 10, 00, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 11, 00, 00),
                IsCancelled = false
            };

            Appointment appointment2 = new Appointment()
            {
                Id = 4,
                PatientId = "a",
                StudentId = "b",
                DateTime = new DateTime(2027, 01, 21, 10, 30, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 11, 30, 00),
                IsCancelled = false
            };



            CreateAppointmentViewModel model = new CreateAppointmentViewModel()
            {
                Appointment = appointment
            };

            CreateAppointmentViewModel model2 = new CreateAppointmentViewModel()
            {
                Appointment = appointment2
            };

            var appointmentRepo = helper.GetInMemoryAppointmentRepo();

            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();


            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);

            await sut.Create(model);
            await sut.Create(model2);

            var result = appointmentRepo.GetAppointments().ToList().Count();

            //assert only the first appointment has been added
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Create_Appointment_Should_Return_Error_When_Max_Amount_Of_Appointments_Has_Been_Reached()
        {
            
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();   

            //if max amount of appointments has been reached, throw error
            //max amount of appointments = 2

            Appointment appointment = new Appointment()
            {
                Id = 5,
                PatientId = "a",
                StudentId = "b",
                DateTime = new DateTime(2027, 01, 21, 10, 00, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 11, 00, 00),
                IsCancelled = false
            };

            Appointment appointment2 = new Appointment()
            {
                Id = 6,
                PatientId = "a",
                StudentId = "b",
                DateTime = new DateTime(2027, 01, 21, 12, 00, 00), //this is a valid datetime.
                EndTime = new DateTime(2027, 01, 21, 13, 00, 00),
                IsCancelled = false
            };
            //this will be the object the test will do checks on
            Appointment appointment3 = new Appointment()
            {
                Id = 7,
                PatientId = "a",
                StudentId = "b",
                DateTime = new DateTime(2027, 01, 21, 14, 00, 00), //this is a valid datetime but the maximum amount of appointments this week has been reached
                EndTime = new DateTime(2027, 01, 21, 15, 00, 00),
                IsCancelled = false
            };

            CreateAppointmentViewModel model = new CreateAppointmentViewModel()
            {
                Appointment = appointment
            };

            CreateAppointmentViewModel model2 = new CreateAppointmentViewModel()
            {
                Appointment = appointment2
            };

            CreateAppointmentViewModel model3 = new CreateAppointmentViewModel()
            {
                Appointment = appointment3
            };

            var appointmentRepo = helper.GetInMemoryAppointmentRepo();
            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();

            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);

            await sut.Create(model); //will work
            await sut.Create(model2); // will work
            await sut.Create(model3); // will fail, because the maximum amount of appointments is 2

            var result = appointmentRepo.GetAppointments().ToList().Count();

            //assert only the first two appointments have been added
            Assert.Equal(2, result);
        }

        [Fact]
        public async void Cancel_Appointment_Should_Return_Error_When_Appointment_Is_Within_24_Hours()
        {
           
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();            

            Appointment appointment = new Appointment()
            {
                Id = 8,
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddHours(14), //this is a valid datetime.
                EndTime = DateTime.Now.Date.AddHours(15),
                IsCancelled = false
            };

            var appointmentRepo = helper.GetInMemoryAppointmentRepo();
            appointmentRepo.CreateAppointment(appointment);
            appointmentRepo.Save();

            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();
            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);

            await sut.Cancel(appointment.Id); //will fail, because the appointment is within 24 hours

            var foundAppointment = appointmentRepo.GetAppointment(appointment.Id).FirstOrDefault();
            var result = foundAppointment.IsCancelled;

            Assert.False(result);

        }

        [Fact]
        public async void Cancel_Appointment_Should_Cancel_When_Appointment_Is_Not_Within_24_Hours()
        {
           
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();  
            
            Appointment appointment = new Appointment()
            {
                Id = 9,
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddDays(25), //this is a valid datetime.
                EndTime = DateTime.Now.Date.AddDays(25),
                IsCancelled = false
            };

            var appointmentRepo = helper.GetInMemoryAppointmentRepo();
            appointmentRepo.CreateAppointment(appointment);
            appointmentRepo.Save();

            var mockIdentityuserRepo = new Mock<IIdentityUserRepository>();
            var sut = new AppointmentsController(appointmentRepo, mockIdentityuserRepo.Object, studentRepo, patientRepo, teacherRepo, fileRepo);

            await sut.Cancel(appointment.Id);

            var foundAppointment = appointmentRepo.GetAppointment(appointment.Id).FirstOrDefault();
            Console.WriteLine(foundAppointment);
            var result = foundAppointment.IsCancelled;

            //assert IsCancelled for the appointment is still false
            Assert.True(result);

        }

    }
}
