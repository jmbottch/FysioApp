using FysioApp.Data;
using FysioApp.Models;
using FysioApp.Models.ApplicationUsers;
using FysioApp.Models.ViewModels.AppointmentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _identity;
        private readonly BusinessDbContext _db;

        public AppointmentsController(ApplicationDbContext identity, BusinessDbContext db)
        {
            _identity = identity;
            _db = db;

        }

        //GET for Index
        public async Task<IActionResult> Index()
        {
            return View(await _db.Appointment.Include(a => a.Teacher).Include(a => a.Patient).ToListAsync());
        }

        //Get for Create
        public IActionResult Create()
        {
            CreateAppointmentViewModel CreateVM = new CreateAppointmentViewModel()
            {
                Appointment = new Appointment()
                {
                    DateTime = DateTime.Now
                },
                //Teachers = _identity.Teacher.ToList(),
                //Patients = _identity.Patient.ToList()
            };

            return View(CreateVM);
        }

        //POST for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {

            if (ModelState.IsValid)
            {
                //var appointment = new Appointment()
                //{
                //    Description = model.Appointment.Description,
                //    DateTime = model.Appointment.DateTime,
                //    PatientId = model.Appointment.PatientId,
                //    TeacherId = model.Appointment.TeacherId
                //};

                _db.Add(model.Appointment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CreateAppointmentViewModel modelVM = new CreateAppointmentViewModel()
            {
                //Teachers = _identity.Teacher.ToList(),
                //Patients = _identity.Patient.ToList(),
                Appointment = model.Appointment,

            };
            return View(model);

        }
    }
}
