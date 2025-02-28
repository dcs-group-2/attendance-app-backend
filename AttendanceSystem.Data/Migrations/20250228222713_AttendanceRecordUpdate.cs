using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceRecordUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Record",
                table: "AttendanceRecord",
                newName: "TeacherAttendance");

            migrationBuilder.AddColumn<int>(
                name: "StudentAttendance",
                table: "AttendanceRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StudentSubmitted",
                table: "AttendanceRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TeacherSubmitted",
                table: "AttendanceRecord",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentAttendance",
                table: "AttendanceRecord");

            migrationBuilder.DropColumn(
                name: "StudentSubmitted",
                table: "AttendanceRecord");

            migrationBuilder.DropColumn(
                name: "TeacherSubmitted",
                table: "AttendanceRecord");

            migrationBuilder.RenameColumn(
                name: "TeacherAttendance",
                table: "AttendanceRecord",
                newName: "Record");
        }
    }
}
