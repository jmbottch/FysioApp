using FysioApp.Data;
using FysioApp.Models.ApplicationUsers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using FysioApp.Utility;
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
        private readonly ApplicationDbContext _identity;
        private readonly BusinessDbContext _business;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterPatientViewModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PatientsController(ApplicationDbContext identity, BusinessDbContext business, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<RegisterPatientViewModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _identity = identity;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _business = business;
        }
        //GET For Index
        public async Task<IActionResult> Index()
        {
            return View(await _business.Patient.ToListAsync());
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

            var patient = await _business.Patient.SingleOrDefaultAsync(t => t.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //Get for Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _business.Patient.SingleOrDefaultAsync(t => t.Id == id);
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

            var patient = await _business.Patient.SingleOrDefaultAsync(t => t.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //POST for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterPatientViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var identityPatient = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };



                var result = await _userManager.CreateAsync(identityPatient, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Student has created a new account with password.");
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.TeacherEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.TeacherEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.StudentEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.StudentEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.PatientEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.PatientEndUser));
                    }

                    await _userManager.AddToRoleAsync(identityPatient, StaticDetails.PatientEndUser);

                    var patientFromDb = _identity.Users.Where(p => p.Email == model.Email).FirstOrDefault();
                    var patient = new Patient()
                    {
                        Id = patientFromDb.Id,
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

                    _business.Patient.Add(patient);
                    await _business.SaveChangesAsync();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {

                        await _signInManager.SignInAsync(identityPatient, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        //POST for Edit
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
            var identityPatientFromDb = await _identity.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
            if(identityPatientFromDb == null)
            {
                return NotFound();
            }           
            var patientFromDb = await _business.Patient.Where(t => t.Id == id).FirstOrDefaultAsync();
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
                await _business.SaveChangesAsync();
                await _identity.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        //POST action for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var patient = await _identity.Users.SingleOrDefaultAsync(t => t.Id == id);
            _identity.Users.Remove(patient);
            await _identity.SaveChangesAsync();
            var businessPatient = await _business.Patient.SingleOrDefaultAsync(p => p.Id == id);
            _business.Patient.Remove(businessPatient);
            await _business.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
