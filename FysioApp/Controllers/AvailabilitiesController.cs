using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using ApplicationCore.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class AvailabilitiesController : Controller
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepostitory _studentRepostitory;
        public AvailabilitiesController(
            IAvailabilityRepository availabilityRepository,
            ITeacherRepository teacherRepository,
            IStudentRepostitory studentRepository
            )
        {
            _availabilityRepository = availabilityRepository;
            _teacherRepository = teacherRepository;
            _studentRepostitory = studentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            Availability availabilty = new Availability();
            return View(availabilty);

        }

        public async Task<IActionResult> Edit(int id)
        {
            Availability availability = _availabilityRepository.GetAvailability(id).FirstOrDefault();
            return View(availability);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Availability availability)
        {
            _availabilityRepository.CreateAvailability(availability);
            _availabilityRepository.Save();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Availability model)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (ModelState.IsValid)
            {
                if (User.IsInRole(StaticDetails.TeacherEndUser) || User.IsInRole(StaticDetails.StudentEndUser)) 
                {
                    Availability availabilityFromDb = _availabilityRepository.GetAvailability(model.Id).FirstOrDefault();
                    availabilityFromDb.MondayStart = model.MondayStart;
                    availabilityFromDb.MondayEnd = model.MondayEnd;
                    availabilityFromDb.TuesdayStart = model.TuesdayStart;
                    availabilityFromDb.TuesdayEnd = model.TuesdayEnd;
                    availabilityFromDb.WednesdayStart = model.WednesdayStart;
                    availabilityFromDb.WednesdayEnd = model.WednesdayEnd;
                    availabilityFromDb.ThursdayStart = model.ThursdayStart;
                    availabilityFromDb.ThursdayEnd = model.ThursdayEnd;
                    availabilityFromDb.FridayStart = model.FridayStart;
                    availabilityFromDb.FridayEnd = model.FridayEnd;
                    _availabilityRepository.Save();
                    if (User.IsInRole(StaticDetails.TeacherEndUser))
                    {
                        return RedirectToAction("Details", "Teachers", new { id = userId });
                    }
                    if (User.IsInRole(StaticDetails.StudentEndUser))
                    {
                        return RedirectToAction("Details", "Students", new { id = userId });
                    }
                } else
                {
                    ModelState.AddModelError(string.Empty, "U bent niet bevoegd om dit aan te passen.");
                }
                
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Availability model)
        //{    
        //    if (ModelState.IsValid)
        //    {     
        //        _availabilityRepository.CreateAvailability(model); //create new appointment with object properties
        //        _availabilityRepository.Save();                      //save to db
        //        int AvailabilityId = model.Id;
        //        var claimsIdentity = (ClaimsIdentity)this.User.Identity;
        //        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        if (User.IsInRole(StaticDetails.TeacherEndUser)) //if loggedinuser = teacher
        //        {
        //            //find teacher
        //            Teacher teacher =_teacherRepository.GetTeacher(userId).FirstOrDefault();
        //            teacher.AvailabilityId = AvailabilityId;

        //        }
        //        if (User.IsInRole(StaticDetails.StudentEndUser)) //if loggedinuser = student
        //        {
                    
        //        }
        //        return RedirectToAction(nameof(Index));                //redirect to list of appointments
        //    }
        //    return View(model);
        //}

        //public IActionResult Create()
        //{
        //    _availabilityRepository.CreateAvailability(new Availability());
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
