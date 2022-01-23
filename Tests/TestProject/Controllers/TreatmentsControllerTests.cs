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
        Testhelper helper = new Testhelper();

        [Fact]
        public async void Create_Treatment_Should_Create_When_Explanation_Is_Required_And_Explanaion_Is_Given()
        {            

            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();            

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 10,
                    Code = "1000", //explanation required
                    Explanation = "TestExplanation", //explanation given
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = 20
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment);

            var result = treatmentRepo.GetTreatmentsByPatientFileId(20).Count();

            Assert.Equal(1, result);
        }
        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Explanation_Is_Required_And_No_Explanaion_Is_Given()
        {
          
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 11,
                    Code = "1000", // explanation required
                    Explanation = null, //not given
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(5),
                    StudentId = "b",
                    PatientFileId = 20
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();

            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(20).Count();

            Assert.Equal(0, result);
        }

        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Patient_Is_Not_Yet_Registered()
        {
           
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 14,
                    Code = "1101",
                    Explanation = null,
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(1), //this is before the patient is registered
                    StudentId = "b",
                    PatientFileId = 20
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();
            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment);

            var result = treatmentRepo.GetTreatmentsByPatientFileId(20).Count();

            Assert.Equal(0, result);
        }

        [Fact]
        public async void Create_Treatment_Should_Return_Error_When_Patient_Is_Not_Registered_Anymore()
        {
            
            var availabilityRepo = helper.GetInMemoryAvailabilityRepo();
            var studentRepo = helper.GetInMemoryStudentRepo();
            var teacherRepo = helper.GetInMemoryTeacherRepo();
            var patientRepo = helper.GetInMemoryPatientRepo();
            var fileRepo = helper.GetInMemoryPatientFileRepo();          

            CreateTreatmentViewModel treatment = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment()
                {
                    Id = 15,
                    Code = "1101",
                    Explanation = null,
                    Room = "Oefenzaal",
                    DateTime = DateTime.Now.AddYears(9), //this is when the patient is not registered anymore
                    StudentId = "b",
                    PatientFileId = 20
                }
            };

            var treatmentRepo = helper.GetInMemoryTreatmentRepo();
            var sut = new TreatmentsController(treatmentRepo, studentRepo, teacherRepo, fileRepo);

            await sut.Create(treatment); //wil api call maken maar dat kan niet omdat deze niet runt....

            var result = treatmentRepo.GetTreatmentsByPatientFileId(20).Count();

            Assert.Equal(0, result);
        }
    }
}
