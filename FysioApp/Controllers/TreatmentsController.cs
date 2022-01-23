using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using FysioApp.Models.ViewModels.TreatmentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class TreatmentsController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ITreatmentRepository _treatmentsRepository;
        private readonly IStudentRepostitory _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPatientFileRepository _patientFileRepository;

        public TreatmentsController
            (
            ITreatmentRepository treatmentsRepository,
            IStudentRepostitory studentRepostitory,
            ITeacherRepository teacherRepository,
            IPatientFileRepository patientFileRepository
            )
        {
            _treatmentsRepository = treatmentsRepository;
            _studentRepository = studentRepostitory;
            _teacherRepository = teacherRepository;
            _patientFileRepository = patientFileRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _treatmentsRepository.GetTreatments().ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _treatmentsRepository.GetTreatment(id).FirstOrDefaultAsync());
        }

        //get actions for create/edit/delete

        public async Task<IActionResult> Create(int patientfileid)
        {
            IEnumerable<Operation> operations = new List<Operation>();
            HttpResponseMessage response = await client.GetAsync("http://myfysiowebapi.azurewebsites.net/api/Operations");
            if (response.IsSuccessStatusCode)
            {
                operations = await response.Content.ReadAsAsync<IEnumerable<Operation>>();
            }

            CreateTreatmentViewModel vm = new CreateTreatmentViewModel()
            {
                Treatment = new Treatment() {
                    DateTime = DateTime.Now,
                    PatientFileId = patientfileid
                },
                Operations = operations,
                Students = _studentRepository.GetStudents().ToList(),
                Teachers = _teacherRepository.GetTeachers().ToList()
            };

            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            IEnumerable<Operation> operations = new List<Operation>();
            HttpResponseMessage response = await client.GetAsync("http://myfysiowebapi.azurewebsites.net/api/Operations");
            if (response.IsSuccessStatusCode)
            {
                operations = await response.Content.ReadAsAsync<IEnumerable<Operation>>();
            }

            Treatment treatment = await _treatmentsRepository.GetTreatment(id).FirstOrDefaultAsync();
            if(treatment == null)
            {
                return NotFound();
            } else
            {
                CreateTreatmentViewModel vm = new CreateTreatmentViewModel()
                {
                    Operations = operations,
                    Treatment = treatment,
                    Students = _studentRepository.GetStudents().ToList(),
                    Teachers = _teacherRepository.GetTeachers().ToList()
                };
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Treatment treatment = await _treatmentsRepository.GetTreatment(id).FirstOrDefaultAsync();
            if(treatment == null)
            {
                return NotFound();
            } else
            {
                return View(treatment);
            }
        }

        //POST ACTIONS for create/edit/delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTreatmentViewModel model)
        {
            //api call for retrieving operations, for when the form fails.
            IEnumerable<Operation> operations = new List<Operation>();
            HttpResponseMessage response = await client.GetAsync("http://myfysiowebapi.azurewebsites.net/api/Operations");
            if (response.IsSuccessStatusCode)
            {
                operations = await response.Content.ReadAsAsync<IEnumerable<Operation>>();
            }
            //new viewModel for when the form fails
            CreateTreatmentViewModel vm = new CreateTreatmentViewModel()
            {
                Operations = operations,
                Treatment = model.Treatment,
                Students = _studentRepository.GetStudents().ToList(),
                Teachers = _teacherRepository.GetTeachers().ToList()
            };
            //api call for retrieving single operation. When one is selected and submitted in the form, this operation will find the correct object.
            string url = "http://myfysiowebapi.azurewebsites.net/api/Operations/" + model.Treatment.Code;
            Operation operation = new Operation();
            HttpResponseMessage response1 = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                operation = await response1.Content.ReadAsAsync<Operation>();
                model.Treatment.Description = operation.Description;
            }
            
            //create new Treatment object
            Treatment treatment = new Treatment()
            {
                Code = model.Treatment.Code,
                Description = model.Treatment.Description,
                Room = model.Treatment.Room,
                DateTime = model.Treatment.DateTime,
                StudentId = model.Treatment.StudentId,
                PatientFileId = model.Treatment.PatientFileId
            };

            //check for valid ModelState
            if(ModelState.IsValid)
            {
                PatientFile patientFileFromDb = await _patientFileRepository.GetFile(model.Treatment.PatientFileId).FirstOrDefaultAsync();
                if (model.Treatment.DateTime > patientFileFromDb.DateOfDeparture || model.Treatment.DateTime < patientFileFromDb.DateOfArrival)
                {
                    ModelState.AddModelError(string.Empty, "Op dit moment is de patient niet ingeschreven."); //if yes but no explanation is given
                    return View(vm);
                }
                if (operation.DescriptionRequired == true)
                {
                    if(model.Treatment.Explanation != null)
                    {
                        treatment.Explanation = model.Treatment.Explanation;
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Toelichting is verplicht voor deze behandeling."); //if yes but no explanation is given
                        return View(vm); // return view with error
                    }
                }
                else
                {
                    var expl = "None"; //set default explanation none
                    if (model.Treatment.Explanation != null) //if the user filled in an explanation anyway
                    {
                        expl = model.Treatment.Explanation; // set the new explanation
                    }
                    treatment.Explanation = expl;
                }

                _treatmentsRepository.CreateTreatment(treatment);
                _treatmentsRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateTreatmentViewModel model)
        {
            //api call for retrieving operations, for when the form fails.
            IEnumerable<Operation> operations = new List<Operation>();
            HttpResponseMessage response = await client.GetAsync("http://myfysiowebapi.azurewebsites.net/api/Operations");
            if (response.IsSuccessStatusCode)
            {
                operations = await response.Content.ReadAsAsync<IEnumerable<Operation>>();
            }
            //new viewModel for when the form fails
            CreateTreatmentViewModel vm = new CreateTreatmentViewModel()
            {
                Operations = operations,
                Treatment = model.Treatment,
                Students = _studentRepository.GetStudents().ToList(),
                Teachers = _teacherRepository.GetTeachers().ToList()
            };
            //api call for retrieving single operation. When one is selected and submitted in the form, this operation will find the correct object.
            string url = "http://myfysiowebapi.azurewebsites.net/api/Operations/" + model.Treatment.Code;
            Operation operation = new Operation();
            HttpResponseMessage response1 = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                operation = await response1.Content.ReadAsAsync<Operation>();
                model.Treatment.Description = operation.Description;
            }

            if(ModelState.IsValid)
            {
                Treatment treatmentFromDb = await _treatmentsRepository.GetTreatment(model.Treatment.Id).FirstOrDefaultAsync();
                if(treatmentFromDb == null)
                {
                    return NotFound();
                } else if(DateTime.Now.Date > treatmentFromDb.DateTime.Date)
                {
                    ModelState.AddModelError(string.Empty, "U kunt de behandeling niet meer wijzigen."); //Error, You cant change it anymore
                    return View(model);
                } else
                {
                    treatmentFromDb.Code = model.Treatment.Code;
                    treatmentFromDb.Description = model.Treatment.Description;
                    treatmentFromDb.Room = model.Treatment.Room;
                    treatmentFromDb.DateTime = model.Treatment.DateTime;
                    treatmentFromDb.StudentId = model.Treatment.StudentId;

                    if (operation.DescriptionRequired == true)
                    {
                        if (model.Treatment.Explanation != "None")
                        {
                            treatmentFromDb.Explanation = model.Treatment.Explanation;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Toelichting is verplicht voor deze behandeling."); //if yes but no explanation is given
                            return View(vm); // return view with error
                        }
                    }
                    else
                    {
                        var expl = "None"; //set default explanation none
                        if (model.Treatment.Explanation != "None") //if the user filled in an explanation anyway
                        {
                            expl = model.Treatment.Explanation; // set the new explanation
                        }
                        treatmentFromDb.Explanation = expl;
                    }

                    _treatmentsRepository.Save();
                    return RedirectToAction(nameof(Index));
                }                
            }
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _treatmentsRepository.DeleteTreatment(id);
            _treatmentsRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
