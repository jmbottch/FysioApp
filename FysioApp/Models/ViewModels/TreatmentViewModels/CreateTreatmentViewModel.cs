using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.ApplicationUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ViewModels.TreatmentViewModels
{
    public class CreateTreatmentViewModel
    {
        public IEnumerable<Operation> Operations { get; set; }
        public Treatment Treatment { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
