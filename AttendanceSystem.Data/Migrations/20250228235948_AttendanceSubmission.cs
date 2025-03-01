using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherSubmitted",
                table: "AttendanceRecord",
                newName: "AttendanceSubmission_TeacherSubmitted");

            migrationBuilder.RenameColumn(
                name: "TeacherAttendance",
                table: "AttendanceRecord",
                newName: "AttendanceSubmission_TeacherAttendance");

            migrationBuilder.RenameColumn(
                name: "StudentSubmitted",
                table: "AttendanceRecord",
                newName: "AttendanceSubmission_StudentSubmitted");

            migrationBuilder.RenameColumn(
                name: "StudentAttendance",
                table: "AttendanceRecord",
                newName: "AttendanceSubmission_StudentAttendance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttendanceSubmission_TeacherSubmitted",
                table: "AttendanceRecord",
                newName: "TeacherSubmitted");

            migrationBuilder.RenameColumn(
                name: "AttendanceSubmission_TeacherAttendance",
                table: "AttendanceRecord",
                newName: "TeacherAttendance");

            migrationBuilder.RenameColumn(
                name: "AttendanceSubmission_StudentSubmitted",
                table: "AttendanceRecord",
                newName: "StudentSubmitted");

            migrationBuilder.RenameColumn(
                name: "AttendanceSubmission_StudentAttendance",
                table: "AttendanceRecord",
                newName: "StudentAttendance");
        }
    }
}
