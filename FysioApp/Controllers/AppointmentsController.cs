using FysioApp.Abstractions;
using FysioApp.Data;
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
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IStudentRepostitory _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentsController(
            IAppointmentRepository appointmentRepository, 
            IIdentityUserRepository identityUserRepository,
            IStudentRepostitory studentRepostitory,
            IPatientRepository patientRepository,
            ITeacherRepository teacherRepository
            )
        {
            _appointmentRepository = appointmentRepository;
            _identityUserRepository = identityUserRepository;
            _studentRepository = studentRepostitory;
            _teacherRepository = teacherRepository;
            _patientRepository = patientRepository;

        }

        //GET for Index
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(StaticDetails.PatientEndUser))
            {

                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                return View(await _appointmentRepository.GetAppointmentsByPatientId(userId).ToListAsync());

            }

            return View(await _appointmentRepository.GetAppointments().ToListAsync());
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
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList()
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
            Appointment appointment = await _appointmentRepository.GetAppointment(id).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        //Get for Edit
        public async Task<IActionResult> Edit(int id)
        {
            Appointment appointment = await _appointmentRepository.GetAppointment(id).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return NotFound();
            }
            CreateAppointmentViewModel EditVM = new CreateAppointmentViewModel()
            {
                Appointment = appointment,
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList()
            };
            return View(EditVM);
        }

        //Get for Delete
        public async Task<IActionResult> Delete(int id)
        {
            Appointment appointment = await _appointmentRepository.GetAppointment(id).FirstOrDefaultAsync();
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

                    _appointmentRepository.CreateAppointment(appointment);
                    _appointmentRepository.Save();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    ModelState.AddModelError(string.Empty, "Datum moet in de toekomst liggen");
                }                   

                
            }

            CreateAppointmentViewModel modelVM = new CreateAppointmentViewModel()
            {                
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList(),
                Appointment = model.Appointment

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
                Appointment appointmentFromDb = await _appointmentRepository.GetAppointment(model.Appointment.Id).FirstOrDefaultAsync();
                if (appointmentFromDb == null)
                {
                    return NotFound();
                }
                appointmentFromDb.Description = model.Appointment.Description;
                appointmentFromDb.DateTime = model.Appointment.DateTime;
                appointmentFromDb.StudentId = model.Appointment.StudentId;
                appointmentFromDb.PatientId = model.Appointment.PatientId;

                _appointmentRepository.Save();
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {

            Appointment appointment = await _appointmentRepository.GetAppointment(id).FirstOrDefaultAsync();
            appointment.IsCancelled = true;
            _appointmentRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        //POST for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _appointmentRepository.DeleteAppointment(id);
            _appointmentRepository.Save();           
            return RedirectToAction(nameof(Index));
        }
    }
}
