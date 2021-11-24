using FysioApp.Data;
using FysioApp.Models;
using FysioApp.Models.ViewModels.PatientFileViewModels;
using FysioApp.Utility;
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
        private readonly BusinessDbContext _business;

        public PatientFilesController(BusinessDbContext business)
        {
            _business = business;
        }
        public async Task<IActionResult> Index()
        {
            var FilesList = await _business.PatientFile.Include(p => p.HeadPractitioner).Include(p => p.Patient).ToListAsync();
            return View(FilesList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var file = await _business.PatientFile.Where(f => f.Id == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy).FirstOrDefaultAsync();
            return View(file);
        }

        public async Task<IActionResult> MyDetails(string id)
        {
            var file = await _business.PatientFile.Where(f => f.PatientId == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy).FirstOrDefaultAsync();
            return View("Details", file);
        }

        public IActionResult Edit(int id)
        {
            var file = _business.PatientFile.Where(f => f.Id == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy).FirstOrDefault();
            if (file == null)
            {
                return NotFound();
            }
            CreatePatientFileViewModel EditVM = new CreatePatientFileViewModel()
            {
                PatientFile = file,
                Students = _business.Student.ToList(),
                Patients = _business.Patient.ToList(),
                Teachers = _business.Teacher.ToList()
            };
            return View(EditVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _business.PatientFile.Where(f => f.Id == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy).FirstOrDefaultAsync();
            return View(file);
        }

        public IActionResult Create()
        {           

            CreatePatientFileViewModel CreateVM = new CreatePatientFileViewModel()
            {
                PatientFile = new PatientFile() 
                {
                
                },
                Students = _business.Student.ToList(),
                Patients = _business.Patient.ToList(),
                Teachers = _business.Teacher.ToList()                
            };           

            return View(CreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientFileViewModel model)
        {
            

            var patient = _business.Patient.Where(p => p.Id == model.PatientFile.PatientId).FirstOrDefault();
            var fileExists = _business.PatientFile.Where(p => p.PatientId == model.PatientFile.PatientId).FirstOrDefault();
            if(fileExists != null)
            {
                ModelState.AddModelError(string.Empty, "Er is reeds een dossier gemaakt voor deze patient");
            }

            model.PatientFile.age = Decimal.ToInt32(((model.PatientFile.DateOfArrival - patient.DateOfBirth).Days) / 365.25m);            

            if (ModelState.IsValid)
            {
                _business.PatientFile.Add(model.PatientFile);
                await _business.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else
            {
                CreatePatientFileViewModel vm = new CreatePatientFileViewModel()
                {
                    PatientFile = model.PatientFile,
                    Students = _business.Student.ToList(),
                    Patients = _business.Patient.ToList(),
                    Teachers = _business.Teacher.ToList()
                };
                return View(vm);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreatePatientFileViewModel model)
        {
            if(ModelState.IsValid)
            {
                var patientFromDb = _business.Patient.Where(p => p.Id == model.PatientFile.PatientId).FirstOrDefault();
                var fileFromDb = await _business.PatientFile.Where(f => f.Id == model.PatientFile.Id).FirstOrDefaultAsync();
                if(fileFromDb == null)
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

                await _business.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else
            {
                CreatePatientFileViewModel vm = new CreatePatientFileViewModel()
                {
                    PatientFile = model.PatientFile,
                    Students = _business.Student.ToList(),
                    Patients = _business.Patient.ToList(),
                    Teachers = _business.Teacher.ToList()
                };
                return View(vm);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _business.PatientFile.Where(p => p.Id == id).FirstOrDefaultAsync();

            _business.PatientFile.Remove(file);
            await _business.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
