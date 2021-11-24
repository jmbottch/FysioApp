﻿using FysioApp.Data;
using FysioApp.Models;
using FysioApp.Models.ApplicationUsers;
using FysioApp.Models.ViewModels.AppointmentViewModels;
using FysioApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _identity;
        private readonly BusinessDbContext _business;

        public AppointmentsController(ApplicationDbContext identity, BusinessDbContext business)
        {
            _identity = identity;
            _business = business;

        }

        //GET for Index
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(StaticDetails.PatientEndUser))
            {

                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                return View(await _business.Appointment.Where(a => a.PatientId == userId).Include(a => a.Student).Include(a => a.Patient).OrderBy(x => x.DateTime).ToListAsync());

            }

            return View(await _business.Appointment.Include(a => a.Student).Include(a => a.Patient).OrderBy(x => x.DateTime).ToListAsync());
        }

        //Get for Create
        public IActionResult Create()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CreateAppointmentViewModel CreateVM = new CreateAppointmentViewModel()
            {
                Appointment = new Appointment()
                {
                    DateTime = DateTime.Now
                },
                Students = _business.Student.ToList(),
                Patients = _business.Patient.ToList()
            };
            if (User.IsInRole(StaticDetails.PatientEndUser))
            {
                CreateVM.Appointment.PatientId = userId;
            }
            if (User.IsInRole(StaticDetails.StudentEndUser))
            {
                CreateVM.Appointment.StudentId = userId;
            }

            return View(CreateVM);
        }

        //Get for Details
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _business.Appointment.Include(a => a.Student).Include(a => a.Patient).SingleOrDefaultAsync(t => t.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        //Get for Edit
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _business.Appointment.Include(a => a.Student).Include(a => a.Patient).SingleOrDefaultAsync(t => t.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            CreateAppointmentViewModel EditVM = new CreateAppointmentViewModel()
            {
                Appointment = appointment,
                Students = _business.Student.ToList(),
                Patients = _business.Patient.ToList()
            };
            return View(EditVM);
        }

        //Get for Delete
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _business.Appointment.Include(a => a.Student).Include(a => a.Patient).SingleOrDefaultAsync(t => t.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        //POST for Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Appointment.DateTime >= DateTime.Now)
                {
                    var appointment = new Appointment()
                    {
                        Description = model.Appointment.Description,
                        DateTime = model.Appointment.DateTime,
                        PatientId = model.Appointment.PatientId,
                        StudentId = model.Appointment.StudentId,
                        IsCancelled = false
                    };

                    _business.Appointment.Add(model.Appointment);
                    await _business.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    ModelState.AddModelError(string.Empty, "Datum moet in de toekomst liggen");
                }                   

                
            }

            CreateAppointmentViewModel modelVM = new CreateAppointmentViewModel()
            {
                Students = _business.Student.ToList(),
                Patients = _business.Patient.ToList(),
                Appointment = model.Appointment,

            };
            return View(modelVM);

        }

        //POST for Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointmentFromDb = await _business.Appointment.FirstOrDefaultAsync(a => a.Id == model.Appointment.Id);
                if (appointmentFromDb == null)
                {
                    return NotFound();
                }
                appointmentFromDb.Description = model.Appointment.Description;
                appointmentFromDb.DateTime = model.Appointment.DateTime;
                appointmentFromDb.StudentId = model.Appointment.StudentId;
                appointmentFromDb.PatientId = model.Appointment.PatientId;

                await _business.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _business.Appointment.SingleOrDefaultAsync(a => a.Id == id);
            appointment.IsCancelled = true;
            await _business.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //POST for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _business.Appointment.SingleOrDefaultAsync(a => a.Id == id);
            _business.Remove(appointment);
            await _business.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
