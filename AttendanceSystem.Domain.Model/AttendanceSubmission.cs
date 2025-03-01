using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Model
{
    [Owned]
    public class AttendanceSubmission
    {

        public required AttendanceKind StudentAttendance { get; set; }
        public required DateTime? StudentSubmitted { get; set; }
        public required AttendanceKind TeacherAttendance { get; set; }
        public required DateTime? TeacherSubmitted { get; set; }
    }
}

