using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Record",
                table: "AttendanceRecord",
                newName: "TeacherSubmission_Attendance");

            migrationBuilder.AddColumn<int>(
                name: "StudentSubmission_Attendance",
                table: "AttendanceRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StudentSubmission_Submitted",
                table: "AttendanceRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TeacherSubmission_Submitted",
                table: "AttendanceRecord",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentSubmission_Attendance",
                table: "AttendanceRecord");

            migrationBuilder.DropColumn(
                name: "StudentSubmission_Submitted",
                table: "AttendanceRecord");

            migrationBuilder.DropColumn(
                name: "TeacherSubmission_Submitted",
                table: "AttendanceRecord");

            migrationBuilder.RenameColumn(
                name: "TeacherSubmission_Attendance",
                table: "AttendanceRecord",
                newName: "Record");
        }
    }
}
