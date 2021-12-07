using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApiEntities
{
    public class Diagnose
    {
        [Key]
        public string Code { get; set; }
        public string BodyLocalization { get; set; }
        public string Pathology { get; set; }
    }
}
