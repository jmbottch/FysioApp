using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class Testhelper
    {
        private readonly BusinessDbContext context;
        public Testhelper()
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContext>();
            builder.UseInMemoryDatabase(databaseName: "LibraryDbInMemory");

            var dbContextOptions = builder.Options;
            context = new BusinessDbContext(dbContextOptions);
            // Delete existing db before creating a new one
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public IAvailabilityRepository GetInMemoryAvailabilityRepo()
        {
            return new AvailabilityRepository(context);
        }

        public ITeacherRepository GetInMemoryTeacherRepo()
        {
            return new TeacherRepository(context);
        }
        public IStudentRepostitory GetInMemoryStudentRepo()
        {
            return new StudentRepository(context);
        }
        public IPatientRepository GetInMemoryPatientRepo()
        {
            return new PatientRepository(context);
        }
        public IPatientFileRepository GetInMemoryPatientFileRepo()
        {
            return new PatientFileRepository(context);
        }
        public IAppointmentRepository GetInMemoryAppointmentRepo()
        {
            return new AppointmentRepository(context);
        }

        public ITreatmentRepository GetInMemoryTreatmentRepo()
        {
            return new TreatmentRepository(context);
        }

        public Patient CreateTestPatient()
        {
            Patient patient = new Patient()
            {
                Id = "a",
                FirstName = "Test",
                LastName = "Patient",
                Email = "TestPatient@email.com",
                PhoneNumber = "0601010101",
                AvansNumber = 111111,
                AvansRole = "1",
                Picture = null,
                Gender = "0",
                DateOfBirth = new DateTime(1998, 08, 30)
            };

            return patient;
        }

        public Student CreateTestStudent()
        {
            Student student = new Student()
            {
                Id = "b",
                FirstName = "Test",
                LastName = "Student",
                Email = "TestStudent@email.com",
                PhoneNumber = "0611111111",
                StudentNumber = 111,
                AvailabilityId = 1
            };

            return student;
        }

        public Teacher CreateTestTeacher()
        {
            Teacher teacher = new Teacher()
            {
                Id = "c",
                FirstName = "Test",
                LastName = "Teacher",
                Email = "TestTeacher@email.com",
                PhoneNumber = "0611111111",
                EmployeeNumber = 123456,
                BigNumber = 321654

            };

            return teacher;
        }

        public Availability CreateTestAvailability()
        {
            Availability availability = new Availability()
            {
                Id = 1,
                MondayStart = "09:00",
                TuesdayStart = "09:00",
                WednesdayStart = "09:00",
                ThursdayStart = "09:00",
                FridayStart = "09:00",
                MondayEnd = "17:00",
                TuesdayEnd = "17:00",
                WednesdayEnd = "17:00",
                ThursdayEnd = "17:00",
                FridayEnd = "17:00"

            };

            return availability;
        }

        public PatientFile CreateTestFile()
        {
            PatientFile file = new PatientFile()
            {
                PatientId = "a",
                age = 23,
                ComplaintsDescription = "SomeDescription",
                IntakeDoneById = "b",
                IntakeSupervisedById = "c",
                HeadPractitionerId = "b",
                DateOfArrival = DateTime.Now.Date.AddYears(3),
                DateOfDeparture = DateTime.Now.Date.AddYears(7),
                SessionDuration = 1,
                AmountOfSessionsPerWeek = 2,
                DiagnoseCode = "1101",
                DiagnoseDescription = "TestDescription"
,
            };

            return file;
        }


    }
}
