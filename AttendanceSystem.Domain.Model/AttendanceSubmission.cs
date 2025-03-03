using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Model
{
    [Owned]
    [method: SetsRequiredMembers]
    public record AttendanceSubmission()
    {
        public required AttendanceKind Attendance
        {
            get;
            set
            {
                field = value;
                Submitted = DateTime.Now;
            }
        } = AttendanceKind.Unknown;

        public DateTime? Submitted { get; private set; }

        public bool IsSubmitted => Submitted != null;
    }
}

