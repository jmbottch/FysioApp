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
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _identity;
        private readonly BusinessDbContext _business;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterStudentViewModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StudentsController(
            ApplicationDbContext identity,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterStudentViewModel> logger,
            RoleManager<IdentityRole> roleManager,
            BusinessDbContext business
            )
        {
            _business = business;
            _identity = identity;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        //GET for Index
        public async Task<IActionResult> Index()
        {
            return View(await _business.Student.ToListAsync());
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

            var student = await _business.Student.SingleOrDefaultAsync(t => t.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        //Get for Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _business.Student.SingleOrDefaultAsync(t => t.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        //Get for Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _business.Student.SingleOrDefaultAsync(t => t.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        //POST for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterStudentViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {

                var identityStudent = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(identityStudent, model.Password);
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

                    await _userManager.AddToRoleAsync(identityStudent, StaticDetails.StudentEndUser);

                    var studentFromDb = _identity.Users.Where(p => p.Email == model.Email).FirstOrDefault();
                    var student = new Student()
                    {
                        Id = studentFromDb.Id,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        StudentNumber = model.StudentNumber
                    };
                    _business.Student.Add(student);
                    await _business.SaveChangesAsync();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(identityStudent, isPersistent: false);
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
        public async Task<IActionResult> Edit(string id, Student student)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != student.Id)
            {
                return NotFound();
            }
            var identityStudentFromDb = await _identity.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
            if (identityStudentFromDb == null)
            {
                return NotFound();
            }            
            var studentFromDb = await _business.Student.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (studentFromDb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                identityStudentFromDb.Email = student.Email;
                identityStudentFromDb.UserName = student.Email;
                identityStudentFromDb.NormalizedEmail = student.Email.ToUpper();
                identityStudentFromDb.NormalizedUserName = student.Email.ToUpper();

                studentFromDb.Email = student.Email;
                studentFromDb.FirstName = student.FirstName;
                studentFromDb.LastName = student.LastName;
                studentFromDb.PhoneNumber = student.PhoneNumber;
                studentFromDb.StudentNumber = student.StudentNumber;


                await _identity.SaveChangesAsync();
                await _business.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        //POST action for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var identityStudent = await _identity.Users.SingleOrDefaultAsync(t => t.Id == id);
            _identity.Users.Remove(identityStudent);
            await _identity.SaveChangesAsync();
            var student = await _business.Student.SingleOrDefaultAsync(s => s.Id == id);
            _business.Student.Remove(student);
            await _business.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
