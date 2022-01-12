using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.ApplicationUsers;
using ApplicationCore.Utility;
using FysioApp.Models.ViewModels.AppointmentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IStudentRepostitory _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPatientRepository _patientRepository;

        private static readonly HttpClient client = new HttpClient();

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
        public async Task<IActionResult> Create()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CreateAppointmentViewModel CreateVM = new CreateAppointmentViewModel()
            {
                Appointment = new Appointment()
                {
                    DateTime = DateTime.Now.AddHours(1)
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
            //Create a new viewmodel object, for when the form fails and everything needs t be reloaded into the view
            CreateAppointmentViewModel modelVM = new CreateAppointmentViewModel()
            {
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList(),
                Appointment = model.Appointment
            };

            if (ModelState.IsValid)
            {
                if (model.Appointment.DateTime < DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Datum en Tijd moet in de toekomst liggen.");
                    return View(modelVM);
                }

                // create a new appointment object
                Appointment appointment = new Appointment()
                {
                    Description = model.Appointment.Description,
                    DateTime = model.Appointment.DateTime,
                    PatientId = model.Appointment.PatientId,
                    StudentId = model.Appointment.StudentId,
                    IsCancelled = false,
                };

                _appointmentRepository.CreateAppointment(appointment); //create new appointment with object properties
                _appointmentRepository.Save();                         //save to db
                return RedirectToAction(nameof(Index));                //redirect to list of appointments
            }

            return View(modelVM);

        }

        //POST for Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateAppointmentViewModel model)
        {
            //Create a new viewmodel object, for when the form fails and everything needs t be reloaded into the view
            CreateAppointmentViewModel modelVM = new CreateAppointmentViewModel()
            {
                Students = _studentRepository.GetStudents().ToList(),
                Patients = _patientRepository.GetPatients().ToList(),
                Appointment = model.Appointment

            };

            if (ModelState.IsValid)
            {
                Appointment appointmentFromDb = await _appointmentRepository.GetAppointment(model.Appointment.Id).FirstOrDefaultAsync();
                if (appointmentFromDb == null)
                {
                    return NotFound();
                }
                
                if (DateTime.Now.AddHours(24) > appointmentFromDb.DateTime) //check if appointment is within 24 hours
                {
                    ModelState.AddModelError(string.Empty, "U mag de afspraak niet meer wijzigen, deze vindt plaats binnen 24 uur."); // if not return with error
                    return View(modelVM);
                }

                if (model.Appointment.DateTime < DateTime.Now) //check if datetime is in future
                {
                    ModelState.AddModelError(string.Empty, "Datum en Tijd moet in de toekomst liggen."); // if not return with error
                    return View(modelVM);
                }

                //change most props
                appointmentFromDb.DateTime = model.Appointment.DateTime;
                appointmentFromDb.Description = model.Appointment.Description;  

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
            if (appointment == null)
            {
                return NotFound();
            }
            if (DateTime.Now.AddHours(24) > appointment.DateTime) //check if appointment is within 24 hours
            {
                ModelState.AddModelError(string.Empty, "U mag de afspraak niet meer annuleren, deze vindt plaats binnen 24 uur."); // if not return with error
                return View(appointment);
            }
            
            appointment.IsCancelled = true;
            _appointmentRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        //POST for Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Appointment appointment = await _appointmentRepository.GetAppointment(id).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return NotFound();
            }
            if (DateTime.Now.AddHours(24) > appointment.DateTime) //check if appointment is within 24 hours
            {
                ModelState.AddModelError(string.Empty, "U mag de afspraak niet meer verwijderen, deze vindt plaats binnen 24 uur."); // if not return with error
                return View(appointment);
            }
            _appointmentRepository.DeleteAppointment(id);
            _appointmentRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
