using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        private AttendanceKind _attendance = AttendanceKind.Unknown;
        
        public required AttendanceKind Attendance
        {
            get => _attendance;
            set
            {
                _attendance = value;
                Submitted = DateTime.Now;
            }
        }

        public DateTime? Submitted { get; private set; }

        [NotMapped]
        public bool IsSubmitted => Submitted != null;
    }
}

