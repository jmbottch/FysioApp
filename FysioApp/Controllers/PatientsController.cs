using FysioApp.Abstractions;
using FysioApp.Data;
using FysioApp.Models.ApplicationUsers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using FysioApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterPatientViewModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PatientsController(IIdentityUserRepository identityUserRepository, IPatientRepository patientRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<RegisterPatientViewModel> logger, RoleManager<IdentityRole> roleManager)
        {
            
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _patientRepository = patientRepository;
            _identityUserRepository = identityUserRepository;
        }
        //GET For Index
        public async Task<IActionResult> Index()
        {
            return View(await _patientRepository.GetPatients().ToListAsync());
        }

        //GET for Create
        public IActionResult Create()
        {
            return View();
        }

        //Get for Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = await _patientRepository.GetPatient(id).FirstOrDefaultAsync();
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //Get for Edit
        [Authorize(Roles = StaticDetails.StudentEndUser)]
        //[Authorize(Roles = StaticDetails.TeacherEndUser)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = await _patientRepository.GetPatient(id).FirstOrDefaultAsync();
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //Get for Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = await _patientRepository.GetPatient(id).FirstOrDefaultAsync();
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //POST for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.StudentEndUser)]
        public async Task<IActionResult> Create(RegisterPatientViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    AvansNumber = model.AvansNumber,
                    DateOfBirth = model.DateOfBirth,
                    AvansRole = model.AvansRole,
                    Gender = model.Gender
                };
                var files = HttpContext.Request.Form.Files;
                var lenghtExists = files.Any(x => x.Length > 0);
                if (lenghtExists)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    patient.Picture = p1;

                    

                }
                _patientRepository.CreatePatient(patient);
                _patientRepository.Save();

                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        //POST for Edit
        [Authorize(Roles = StaticDetails.StudentEndUser)]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Patient patient)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != patient.Id)
            {
                return NotFound();
            }
            IdentityUser identityPatientFromDb = await _identityUserRepository.GetUser(id).FirstOrDefaultAsync();
            if (identityPatientFromDb == null)
            {
                return NotFound();
            }
            Patient patientFromDb = await _patientRepository.GetPatient(id).FirstOrDefaultAsync();
            if (patientFromDb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                identityPatientFromDb.Email = patient.Email;
                identityPatientFromDb.UserName = patient.Email;
                identityPatientFromDb.NormalizedEmail = patient.Email.ToUpper();
                identityPatientFromDb.NormalizedUserName = patient.Email.ToUpper();

                patientFromDb.Email = patient.Email;
                patientFromDb.FirstName = patient.FirstName;
                patientFromDb.LastName = patient.LastName;
                patientFromDb.PhoneNumber = patient.PhoneNumber;
                patientFromDb.AvansNumber = patient.AvansNumber;
                patientFromDb.DateOfBirth = patient.DateOfBirth;
                patientFromDb.AvansRole = patient.AvansRole;
                patientFromDb.Gender = patient.Gender;

                var files = HttpContext.Request.Form.Files;
                var lenghtExists = files.Any(x => x.Length > 0);
                if (lenghtExists)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    patientFromDb.Picture = p1;
                }
                _patientRepository.Save();
                _identityUserRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        //POST action for Delete
        [Authorize(Roles = StaticDetails.StudentEndUser)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _identityUserRepository.DeleteUser(id);
            _identityUserRepository.Save();
            _patientRepository.DeletePatient(id);
            _patientRepository.Save();
           
            return RedirectToAction(nameof(Index));
        }
    }
}
