using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using ApplicationCore.Utility;
using FysioApp.Models.ViewModels.PatientFileViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class PatientFilesController : Controller
    {
        private readonly IPatientFileRepository _patientFileRepository;
        private readonly IStudentRepostitory _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPatientRepository _patientRepository;

        public PatientFilesController(
            IPatientFileRepository patientFileRepository,
            IStudentRepostitory studentRepostitory,
            ITeacherRepository teacherRepository,
            IPatientRepository patientRepository
            )
        {
            _patientFileRepository = patientFileRepository;
            _studentRepository = studentRepostitory;
            _teacherRepository = teacherRepository;
            _patientFileRepository = patientFileRepository;
            _patientRepository = patientRepository;
        }
        public async Task<IActionResult> Index()
        {
            var FilesList = await _patientFileRepository.GetFiles().ToListAsync();
            return View(FilesList);
        }

        public async Task<IActionResult> Details(int id)
        {
            PatientFile file = await _patientFileRepository.GetFile(id).FirstOrDefaultAsync();
            List<Comment> comments = await _patientFileRepository.GetCommentsByPatientFileId(id).OrderByDescending(c => c.TimeOfPosting).ToListAsync();
            DetailsPatientFileViewModel vm = new DetailsPatientFileViewModel()
            {
                PatientFile = file,
                Comment = new Comment(),
                Comments = comments
            };
            return View(vm);
        }

        public async Task<IActionResult> MyDetails(string id)
        {
            PatientFile file = await _patientFileRepository.GetFileByPatientId(id).FirstOrDefaultAsync();
            List<Comment> comments = await _patientFileRepository.GetCommentsByPatientFileId(file.Id).OrderByDescending(c => c.TimeOfPosting).ToListAsync();
            DetailsPatientFileViewModel vm = new DetailsPatientFileViewModel()
            {
                PatientFile = file,
                Comment = new Comment(),
                Comments = comments
            };
            return View("Details", vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            PatientFile file = await _patientFileRepository.GetFile(id).FirstOrDefaultAsync();
            if (file == null)
            {
                return NotFound();
            }
            CreatePatientFileViewModel EditVM = new CreatePatientFileViewModel()
            {
                PatientFile = file,
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList(),
                Teachers = _teacherRepository.GetTeachers().ToList()
            };
            return View(EditVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            PatientFile file = await _patientFileRepository.GetFile(id).FirstOrDefaultAsync();
            return View(file);
        }

        public async Task<IActionResult> Create()
        {

            CreatePatientFileViewModel CreateVM = new CreatePatientFileViewModel()
            {
                PatientFile = new PatientFile(),
                Students = await _studentRepository.GetStudents().ToListAsync(),
                Patients = await _patientRepository.GetPatients().ToListAsync(),
                Teachers = await _teacherRepository.GetTeachers().ToListAsync()
            };

            return View(CreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientFileViewModel model)
        {
            Patient patient = await _patientRepository.GetPatient(model.PatientFile.PatientId).FirstOrDefaultAsync();
            var fileExists = await _patientFileRepository.GetFileByPatientId(model.PatientFile.PatientId).FirstOrDefaultAsync();
            if (fileExists != null)
            {
                ModelState.AddModelError(string.Empty, "Er is reeds een dossier gemaakt voor deze patient");
            }

            model.PatientFile.age = Decimal.ToInt32(((model.PatientFile.DateOfArrival - patient.DateOfBirth).Days) / 365.25m);

            if (ModelState.IsValid)
            {
                _patientFileRepository.CreateFile(model.PatientFile);
                _patientFileRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                CreatePatientFileViewModel vm = new CreatePatientFileViewModel()
                {
                    PatientFile = model.PatientFile,
                    Students = await _studentRepository.GetStudents().ToListAsync(),
                    Patients = await _patientRepository.GetPatients().ToListAsync(),
                    Teachers = await _teacherRepository.GetTeachers().ToListAsync()
                };
                return View(vm);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreatePatientFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                Patient patientFromDb = _patientRepository.GetPatient(model.PatientFile.PatientId).FirstOrDefault();
                PatientFile fileFromDb = _patientFileRepository.GetFile(model.PatientFile.Id).FirstOrDefault();
                if (fileFromDb == null)
                {
                    return NotFound();
                }

                fileFromDb.ComplaintsDescription = model.PatientFile.ComplaintsDescription;
                fileFromDb.age = Decimal.ToInt32(((model.PatientFile.DateOfArrival - patientFromDb.DateOfBirth).Days) / 365.25m); ;
                fileFromDb.HeadPractitionerId = model.PatientFile.HeadPractitionerId;
                fileFromDb.IntakeDoneById = model.PatientFile.IntakeDoneById;
                fileFromDb.IntakeSupervisedById = model.PatientFile.IntakeSupervisedById;
                fileFromDb.DateOfDeparture = model.PatientFile.DateOfDeparture;
                fileFromDb.AmountOfSessionsPerWeek = model.PatientFile.AmountOfSessionsPerWeek;
                fileFromDb.SessionDuration = model.PatientFile.SessionDuration;

                _patientFileRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                CreatePatientFileViewModel vm = new CreatePatientFileViewModel()
                {
                    PatientFile = model.PatientFile,
                    Students = await _studentRepository.GetStudents().ToListAsync(),
                    Patients = await _patientRepository.GetPatients().ToListAsync(),
                    Teachers = await _teacherRepository.GetTeachers().ToListAsync()
                };
                return View(vm);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            _patientFileRepository.DeleteFile(id);
            _patientFileRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddComment(DetailsPatientFileViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (User.IsInRole(StaticDetails.StudentEndUser))
            {
                var user = _studentRepository.GetStudent(userId).FirstOrDefault();
                model.Comment.AuthorName = user.FirstName + " " + user.LastName;
            }
            if (User.IsInRole(StaticDetails.TeacherEndUser))
            {
                var user = _teacherRepository.GetTeacher(userId).FirstOrDefault();
                model.Comment.AuthorName = user.FirstName + " " + user.LastName;
            }

            if (ModelState.IsValid)
            {
                _patientFileRepository.AddComment(model.Comment);
                _patientFileRepository.Save();

                DetailsPatientFileViewModel vm = new DetailsPatientFileViewModel()
                {
                    PatientFile = await _patientFileRepository.GetFile(model.Comment.PatientFileId).FirstOrDefaultAsync(),
                    Comment = new Comment()
                    {
                        Content = ""
                    },
                    Comments = await _patientFileRepository.GetCommentsByPatientFileId(model.Comment.PatientFileId).OrderByDescending(c => c.TimeOfPosting).ToListAsync()
                };

                return View(nameof(Details), vm);
            }
            else
            {
                DetailsPatientFileViewModel vm = new DetailsPatientFileViewModel()
                {
                    PatientFile = await _patientFileRepository.GetFile(model.Comment.PatientFileId).FirstOrDefaultAsync(),
                    Comment = model.Comment,
                    Comments = await _patientFileRepository.GetCommentsByPatientFileId(model.Comment.PatientFileId).ToListAsync()
                };
                return View(nameof(Details), vm);
            }

        }
    }
}
