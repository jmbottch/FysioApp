using ApplicationCore.Abstractions;
using ApplicationCore.Entities.ApplicationUsers;
using ApplicationCore.Utility;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
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
        //private readonly SignInManager<IdentityUser> _signInManager;
        ////private readonly UserManager<IdentityUser> _userManager;
        //private readonly ILogger<RegisterPatientViewModel> _logger;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public PatientsController(IIdentityUserRepository identityUserRepository, IPatientRepository patientRepository

            //SignInManager<IdentityUser> signInManager, 
            //UserManager<IdentityUser> userManager, 
            //ILogger<RegisterPatientViewModel> logger, 
            //RoleManager<IdentityRole> roleManager
            )
        {
            
            //_signInManager = signInManager;
            //_userManager = userManager;
            //_logger = logger;
            //_roleManager = roleManager;
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
        [Authorize(Roles = StaticDetails.TeacherEndUser)]
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
        [Authorize(Roles = StaticDetails.TeacherEndUser)]
        public IActionResult Create(RegisterPatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.DateOfBirth.AddYears(16) >= DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Patient is niet ouder dan 16.");
                    return View(model);
                }

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
                if(HttpContext.Request.Form.Files != null)
                {
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
                }
                
                _patientRepository.CreatePatient(patient);
                _patientRepository.Save();

                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        //POST for Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.TeacherEndUser)]
        public async Task<IActionResult> Edit(string id, Patient patient)
        {        
            IdentityUser identityPatientFromDb = await _identityUserRepository.GetUser(id).FirstOrDefaultAsync();
            
            Patient patientFromDb = await _patientRepository.GetPatient(id).FirstOrDefaultAsync();
            
            if (ModelState.IsValid)
            {
                if (patient.DateOfBirth.AddYears(16) >= DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Patient is niet ouder dan 16.");
                    return View(patient);
                }
                if (identityPatientFromDb != null)
                {
                    identityPatientFromDb.Email = patient.Email;
                    identityPatientFromDb.UserName = patient.Email;
                    identityPatientFromDb.NormalizedEmail = patient.Email.ToUpper();
                    identityPatientFromDb.NormalizedUserName = patient.Email.ToUpper();
                }               

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
        [Authorize(Roles = StaticDetails.TeacherEndUser)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _identityUserRepository.DeleteUser(id);
            _identityUserRepository.Save();
            _patientRepository.DeletePatient(id);
            _patientRepository.Save();
           
            return RedirectToAction(nameof(Index));
        }
    }
}
