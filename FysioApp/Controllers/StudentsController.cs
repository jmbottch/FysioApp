using ApplicationCore.Abstractions;
using Infrastructure.Data;
using ApplicationCore.Entities.ApplicationUsers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using ApplicationCore.Utility;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterStudentViewModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStudentRepostitory _studentRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public StudentsController(
            IStudentRepostitory studentRepository,
            IIdentityUserRepository identityUserRepository,            
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterStudentViewModel> logger,
            RoleManager<IdentityRole> roleManager          
            )
        {            
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _studentRepository = studentRepository;
            _identityUserRepository = identityUserRepository;

        }

        //GET for Index
        public async Task<IActionResult> Index()
        {
            return View(await _studentRepository.GetStudents().ToListAsync());
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
            var student = await _studentRepository.GetStudent(id).FirstOrDefaultAsync();
            if(student == null)
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

            var student = await _studentRepository.GetStudent(id).FirstOrDefaultAsync();
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

            var student = await _studentRepository.GetStudent(id).FirstOrDefaultAsync();
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

                    IdentityUser identityStudentFromDb = _identityUserRepository.GetUserByEmail(model.Email).FirstOrDefault();
                    var student = new Student()
                    {
                        Id = identityStudentFromDb.Id,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        StudentNumber = model.StudentNumber
                    };
                    _studentRepository.CreateStudent(student);
                    _studentRepository.Save();

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
            IdentityUser identityStudentFromDb = await _identityUserRepository.GetUser(id).FirstOrDefaultAsync();
            if (identityStudentFromDb == null)
            {
                return NotFound();
            }
            Student studentFromDb = await _studentRepository.GetStudent(id).FirstOrDefaultAsync();
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


                _identityUserRepository.Save();
                _studentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        //POST action for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _identityUserRepository.DeleteUser(id);
            _identityUserRepository.Save();
            _studentRepository.DeleteStudent(id);
            _studentRepository.Save();
            
            
            return RedirectToAction(nameof(Index));
        }

    }
}
