using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApiEntities
{
    public class Operation
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public bool DescriptionRequired { get; set; }
    }
}
