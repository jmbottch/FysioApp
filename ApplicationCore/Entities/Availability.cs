using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Availability
    {
        [Required]
        public int Id { get; set; }
        public string MondayStart { get; set; }
        public string MondayEnd { get; set; }

        public string TuesdayStart { get; set; }
        public string TuesdayEnd { get; set; }

        public string WednesdayStart { get; set; }
        public string WednesdayEnd { get; set; }

        public string ThursdayStart { get; set; }
        public string ThursdayEnd { get; set; }

        public string FridayStart { get; set; }
        public string FridayEnd { get; set; }
    }
}
