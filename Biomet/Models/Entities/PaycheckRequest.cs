using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public class PaycheckRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PayDay { get; set; }
    }
}
