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
        private readonly IPatientFileRepository _patientFileRepository;

        private static readonly HttpClient client = new HttpClient();

        public AppointmentsController(
            IAppointmentRepository appointmentRepository,
            IIdentityUserRepository identityUserRepository,
            IStudentRepostitory studentRepostitory,
            IPatientRepository patientRepository,
            ITeacherRepository teacherRepository,
            IPatientFileRepository patientFileRepository
            )
        {
            _appointmentRepository = appointmentRepository;
            _identityUserRepository = identityUserRepository;
            _studentRepository = studentRepostitory;
            _teacherRepository = teacherRepository;
            _patientRepository = patientRepository;
            _patientFileRepository = patientFileRepository;

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

            //find patientfile to check how long an appointment will last and how many per week are permitted
            PatientFile file = _patientFileRepository.GetFileByPatientId(model.Appointment.PatientId).FirstOrDefault();
            //find student to check availability
            Student student = _studentRepository.GetStudent(model.Appointment.StudentId).FirstOrDefault();
            //find list of already existing appointments on chosen day
            IEnumerable<Appointment> appointments = _appointmentRepository.GetAppointments().Where(a => a.DateTime.Date == model.Appointment.DateTime.Date).ToList();

            

            if (ModelState.IsValid)
            {
                if (model.Appointment.DateTime < DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Datum en Tijd moet in de toekomst liggen.");
                    return View(modelVM);
                }

                //check if doctor already has appointment at this time
                if (appointments != null)
                {
                    foreach (Appointment item in appointments)
                    {                                               
                        if (model.Appointment.DateTime < item.EndTime && model.Appointment.DateTime > item.DateTime) //if new starttime > existing starttime and < existing endtime
                        {
                            
                            ModelState.AddModelError(string.Empty, "De behandelaar heeft al een afspraak op dit tijdstip");
                            return View(modelVM);
                        }
                        if (model.Appointment.DateTime.AddHours(file.SessionDuration) < item.EndTime && model.Appointment.DateTime.AddHours(file.SessionDuration) > item.DateTime) //if new endtime < existing endtime and > existing starttime
                        {
                            ModelState.AddModelError(string.Empty, "De behandelaar heeft al een afspraak op dit tijdstip");
                            return View(modelVM);
                        }
                    }
                }

                //if the weekend is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Saturday" || model.Appointment.DateTime.DayOfWeek.ToString() == "Saturday")
                {
                    ModelState.AddModelError(string.Empty, "De praktijk is gesloten in het weekend.");
                    return View(modelVM);
                }

                //if monday is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Monday")
                {

                    if (DateTime.Parse(model.Appointment.DateTime.TimeOfDay.ToString()) < DateTime.Parse(student.Availability.MondayStart))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te vroeg.");
                        return View(modelVM);
                    }

                    if (DateTime.Parse(model.Appointment.DateTime.AddHours(file.SessionDuration).TimeOfDay.ToString()) > DateTime.Parse(student.Availability.MondayEnd))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te laat.");
                        return View(modelVM);
                    }
                }
                //if tuesday is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Tuesday")
                {

                    if (DateTime.Parse(model.Appointment.DateTime.TimeOfDay.ToString()) < DateTime.Parse(student.Availability.TuesdayStart))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te vroeg.");
                        return View(modelVM);
                    }

                    if (DateTime.Parse(model.Appointment.DateTime.AddHours(file.SessionDuration).TimeOfDay.ToString()) > DateTime.Parse(student.Availability.TuesdayEnd))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te laat.");
                        return View(modelVM);
                    }
                }

                //if wednesday is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Wednesday")
                {

                    if (DateTime.Parse(model.Appointment.DateTime.TimeOfDay.ToString()) < DateTime.Parse(student.Availability.WednesdayStart))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te vroeg.");
                        return View(modelVM);
                    }

                    if (DateTime.Parse(model.Appointment.DateTime.AddHours(file.SessionDuration).TimeOfDay.ToString()) > DateTime.Parse(student.Availability.WednesdayEnd))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te laat.");
                        return View(modelVM);
                    }
                }

                //if Thursday is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Thursday")
                {

                    if (DateTime.Parse(model.Appointment.DateTime.TimeOfDay.ToString()) < DateTime.Parse(student.Availability.ThursdayStart))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te vroeg.");
                        return View(modelVM);
                    }

                    if (DateTime.Parse(model.Appointment.DateTime.AddHours(file.SessionDuration).TimeOfDay.ToString()) > DateTime.Parse(student.Availability.ThursdayEnd))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te laat.");
                        return View(modelVM);
                    }
                }

                //if Friday is chosen
                if (model.Appointment.DateTime.DayOfWeek.ToString() == "Friday")
                {

                    if (DateTime.Parse(model.Appointment.DateTime.TimeOfDay.ToString()) < DateTime.Parse(student.Availability.FridayStart))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te vroeg.");
                        return View(modelVM);
                    }

                    if (DateTime.Parse(model.Appointment.DateTime.AddHours(file.SessionDuration).TimeOfDay.ToString()) > DateTime.Parse(student.Availability.FridayEnd))
                    {
                        ModelState.AddModelError(string.Empty, "Het gekozen tijdstip is te laat.");
                        return View(modelVM);
                    }
                }

                // create a new appointment object
                Appointment appointment = new Appointment()
                {
                    Description = model.Appointment.Description,
                    DateTime = model.Appointment.DateTime,
                    EndTime = model.Appointment.DateTime.AddHours(file.SessionDuration),
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
