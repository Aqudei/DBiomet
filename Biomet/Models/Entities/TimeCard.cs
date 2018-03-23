using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public class TimeCard : EntityBase
    {
        public DateTime LogDate { get; set; }
        public double NumberOfHours { get; set; }

    }
}
