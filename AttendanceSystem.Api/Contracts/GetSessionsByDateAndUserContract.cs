using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Api.Contracts
{
    public class GetSessionsByDateAndUserContract
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }

    }
}
