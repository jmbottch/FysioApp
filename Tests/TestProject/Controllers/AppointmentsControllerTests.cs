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
        //if endtime > availability.Endtime
        [Fact]       
        public async void Create_Appointment_Should_Return_Error_When_Chosen_Time_Is_Too_Late_For_Student()
        {
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();                   
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();   
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();   
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();
         

            //this will be the object the test will do checks on
            //if appointment.endtime > student.availability.endtime, error            
            Appointment appointment = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                //DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(16).AddMinutes(30), //this starting time is too late, appointment will last till 17:30, when student is not available
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
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
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();


            //this will be the object the test will do checks on
            //if appointment.endtime > student.availability.endtime, error

            Appointment appointment = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                //DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(8).AddMinutes(30), //this starting time is too late, appointment will last till 17:30, when student is not available
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
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
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();


            //this will be the object the test will do checks on
            //if appointment.endtime > student.availability.endtime, error

            Appointment appointment = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
                IsCancelled = false
            };

            Appointment appointment2 = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(10).AddMinutes(30), //this is a valid datetime but an appointment has already been planned.
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
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

            //assert no appointment has been added
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Create_Appointment_Should_Return_Error_When_Max_Amount_Of_Appointments_Has_Been_Reached()
        {
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();


            
            //if max amount of appointments has been reached, throw error
            //max amount of appointments = 2

            Appointment appointment = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(10), //this is a valid datetime.
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
                IsCancelled = false
            };

            Appointment appointment2 = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(12).AddMinutes(30), //this is a valid datetime.
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
                IsCancelled = false
            };
            //this will be the object the test will do checks on
            Appointment appointment3 = new Appointment()
            {
                PatientId = "a",
                StudentId = "b",
                DateTime = DateTime.Now.Date.AddYears(5).AddHours(14).AddMinutes(45), //this is a valid datetime but the maximum amount of appointments this week has been reached
                EndTime = DateTime.Now.Date.AddYears(5).AddHours(10),
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

            await sut.Create(model);
            await sut.Create(model2);
            await sut.Create(model3);

            var result = appointmentRepo.GetAppointments().ToList().Count();

            //assert no appointment has been added
            Assert.Equal(2, result);
        }

        [Fact]
        public async void Cancel_Appointment_Should_Return_Error_When_Appointment_Is_Within_24_Hours()
        {
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();



            //if max amount of appointments has been reached, throw error
            //max amount of appointments = 2

            Appointment appointment = new Appointment()
            {
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

            await sut.Cancel(appointment.Id);

            var foundAppointment = appointmentRepo.GetAppointment(appointment.Id).FirstOrDefault();

            var result = foundAppointment.IsCancelled;

            Assert.False(result);

        }

        [Fact]
        public async void Cancel_Appointment_Should_Cancel_When_Appointment_Is_Not_Within_24_Hours()
        {
            var helper = new Testhelper();

            //set up required entities

            Availability availability = helper.CreateTestAvailability();
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            availabilityRepo.CreateAvailability(availability);
            availabilityRepo.Save();

            Student student = helper.CreateTestStudent();
            var studentRepo = helper.GetInMemoryStudentRepo();
            studentRepo.CreateStudent(student);
            studentRepo.Save();

            Teacher teacher = helper.CreateTestTeacher();

            var teacherRepo = helper.GetInMemoryTeacherRepo();
            teacherRepo.CreateTeacher(teacher);
            teacherRepo.Save();

            Patient patient = helper.CreateTestPatient();
            var patientRepo = helper.GetInMemoryPatientRepo();
            patientRepo.CreatePatient(patient);
            patientRepo.Save();

            PatientFile file = helper.CreateTestFile();
            var fileRepo = helper.GetInMemoryPatientFileRepo();
            fileRepo.CreateFile(file);
            fileRepo.Save();



            //if max amount of appointments has been reached, throw error
            //max amount of appointments = 2

            Appointment appointment = new Appointment()
            {
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

            await sut.Cancel(appointment.Id);

            var foundAppointment = appointmentRepo.GetAppointment(appointment.Id).FirstOrDefault();

            var result = foundAppointment.IsCancelled;

            Assert.False(result);

        }

    }
}
