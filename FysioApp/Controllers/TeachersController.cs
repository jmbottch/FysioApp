using ApplicationCore.Abstractions;
using ApplicationCore.Utility;
using ApplicationCore.Entities.ApplicationUsers;
using FysioApp.Models.ViewModels.ApplicationUserViewModels;
using Microsoft.AspNetCore.Http;
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
    public class TeachersController : Controller
    {
        private readonly IIdentityUserRepository _identityRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterTeacherViewModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TeachersController(
            IIdentityUserRepository identityRepository,
            ITeacherRepository teacherRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterTeacherViewModel> logger,
            RoleManager<IdentityRole> roleManager
            )
        {

            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _identityRepository = identityRepository;
            _teacherRepository = teacherRepository;
        }

        //Get for Index
        public async Task<IActionResult> Index()
        {
            return View(await _teacherRepository.GetTeachers().ToListAsync());
        }

        //Get for Create
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
            Teacher teacher = await _teacherRepository.GetTeacher(id).FirstOrDefaultAsync();            
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        //Get for Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher teacher = await _teacherRepository.GetTeacher(id).FirstOrDefaultAsync();
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        //Get for Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher teacher = await _teacherRepository.GetTeacher(id).FirstOrDefaultAsync();
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        //Post for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterTeacherViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                IdentityUser identityTeacher = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };


                var result = await _userManager.CreateAsync(identityTeacher, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Teacher has created a new account with password");
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

                    await _userManager.AddToRoleAsync(identityTeacher, StaticDetails.TeacherEndUser);
                    var teacherFromDb = await _identityRepository.GetUserByEmail(model.Email).FirstOrDefaultAsync();
                    var teacher = new Teacher()
                    {
                        Id = teacherFromDb.Id,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        EmployeeNumber = model.EmployeeNumber,
                        BigNumber = model.BigNumber
                    };

                    _teacherRepository.CreateTeacher(teacher);
                    _teacherRepository.Save();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(identityTeacher, isPersistent: false);
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
        public async Task<IActionResult> Edit(string id, Teacher teacher)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != teacher.Id)
            {
                return NotFound();
            }
            IdentityUser identityTeacherFromDb = await _identityRepository.GetUser(teacher.Id).FirstOrDefaultAsync();
            if (identityTeacherFromDb == null)
            {
                return NotFound();
            }
            Teacher teacherFromDb = await _teacherRepository.GetTeacher(id).FirstOrDefaultAsync();
            if (teacherFromDb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                identityTeacherFromDb.Email = teacher.Email;
                identityTeacherFromDb.UserName = teacher.Email;
                identityTeacherFromDb.NormalizedEmail = teacher.Email.ToUpper();
                identityTeacherFromDb.NormalizedUserName = teacher.Email.ToUpper();

                teacherFromDb.Email = teacher.Email;
                teacherFromDb.FirstName = teacher.FirstName;
                teacherFromDb.LastName = teacher.LastName;
                teacherFromDb.PhoneNumber = teacher.PhoneNumber;
                teacherFromDb.EmployeeNumber = teacher.EmployeeNumber;
                teacherFromDb.BigNumber = teacher.BigNumber;

                _identityRepository.Save();
                _teacherRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        //POST action for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _identityRepository.DeleteUser(id);
            _identityRepository.Save();
            _teacherRepository.DeleteTeacher(id);
            _teacherRepository.Save();
           
            return RedirectToAction(nameof(Index));
        }
    }

}
