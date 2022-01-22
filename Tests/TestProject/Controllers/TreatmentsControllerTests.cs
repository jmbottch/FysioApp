using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using FysioApp.Controllers;
using FysioApp.Models.ViewModels.TreatmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.Controllers
{
    public class TreatmentsControllerTests
    {
        [Fact]
        public async void Create_Treatment_Should_Create_When_Explanation_Is_Required_And_Explanaion_Is_Given()
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

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 1,
                    Code = "1000", //explanation required
                    Explanation = "TestExplanation", //explanation given
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(1, result);
        }
        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Explanation_Is_Required_And_No_Explanaion_Is_Given()
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

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 2,
                    Code = "1000", // explanation required
                    Explanation = null, //not given
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(0, result);
        }
        [Fact]
        public async void Create_Treatment_Should_Create_When_Explanation_Is_Not_Required_And_Explanaion_Is_Given ()
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

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 3,
                    Code = "1101", //expl not required
                    Explanation = "Test Explanation", //given anyway
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(1, result);
        }
        [Fact]
        public async void Create_Treatment_Should_Create_When_Explanation_Is_Not_Required_And_Explanaion_Is_Not_Given()
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


            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 4,
                    Code = "1101", //expl not required
                    Explanation = null,
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(1, result);
        }

        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Patient_Is_Not_Yet_Registered()
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


            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 5,
                    Code = "1101",
                    Explanation = null,
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(1), //this is before the patient is registered
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(0, result);
        }

        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Patient_Is_Not_Registered_Anymore()
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


            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 6,
                    Code = "1101",
                    Explanation = null,
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(9), //this is when the patient is not registered anymore
                    StudentId = "b",
                    PatientFileId = file.Id
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(file.Id).Count();

            Assert.Equal(0, result);
        }
    }
}
