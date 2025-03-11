using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services.Tools
{
    public class MockDataGenerator
    {
        private readonly UserService _userService;
        private readonly CourseService _courseService;
        private readonly AttendanceService _attendanceService;

        private readonly CoursesContext _context;

        public MockDataGenerator(UserService userService, CourseService courseService, AttendanceService attendanceService, CoursesContext context)
        {
            _userService = userService;
            _courseService = courseService;
            _attendanceService = attendanceService;
            _context = context;
        }

        private static readonly List<(string CourseId, string CourseName)> Courses = new List<(string, string)>
    {
        ("CS101", "Introduction to Programming"),
        ("DSA101", "Data Structures and Algorithms"),
        ("DMK101", "Digital Marketing"),
        ("AMT201", "Advanced Mathematics"),
        ("AI301", "Artificial Intelligence"),
        ("DBM202", "Database Management"),
        ("NSC301", "Network Security"),
        ("SE202", "Software Engineering"),
        ("ML301", "Machine Learning"),
        ("WD201", "Web Development"),
        ("CC301", "Cloud Computing"),
        ("BA101", "Business Administration"),
        ("PHI101", "Philosophy 101"),
        ("PSY202", "Psychology and Society"),
        ("ECO101", "Economics"),
        ("GP201", "Global Politics"),
        ("HRM301", "Human Resources Management"),
        ("ES202", "Environmental Science"),
        ("CG301", "Computer Graphics"),
        ("MAD202", "Mobile App Development"),
        ("QC301", "Quantum Computing"),
        ("MLT101", "Modern Literature")
    };


        private static readonly List<string> Departments = new List<string>
    {
        "Computer Science", "Business Administration", "Philosophy", "Psychology", "Engineering", "Environmental Science",
        "Mathematics", "Marketing", "History", "Sociology", "Media Studies", "Economics", "Political Science", "Literature",
        "Education", "Law", "Health Sciences", "Arts", "Architecture", "Data Science", "Chemistry"
    };

        public async Task GenerateRealData()
        {
            // Real Users data from the CSV
            var realUsers = new List<User>
            {
                new User { Id = "01b17f54-f437-46c2-9a1d-3d18b08bedf7", Name = "Maurice Mouw", Email = "maurice.mouw@sue.nl" },
                new User { Id = "10c42de6-8144-44c8-ab33-64d9d86140a9", Name = "Alexander Serraris", Email = "alexander.serraris@student.uva.nl" },
                new User { Id = "14cd03f5-6f8f-49fd-a6ef-ecae6bcbcd82", Name = "Bo ter Ham", Email = "Bo.TerHam@jdekker3008gmail.onmicrosoft.com" },
                new User { Id = "24989931-d191-4a45-96eb-15319b34aa53", Name = "Sam Tudent", Email = "sam.tudent@jdekker3008gmail.onmicrosoft.com" },
                new User { Id = "4c56be3f-67ea-4e52-88d8-68fca8d5e47f", Name = "Lara de Vries", Email = "lara.devries@student.uva.nl" },
                new User { Id = "52f5c67a-89bd-4cd7-9955-bcdb71f3cb67", Name = "Finn Meijer", Email = "finn.meijer@example.com" },
                new User { Id = "6e43a492-b34e-4b3a-bd72-d2f84d88537f", Name = "Isabel van Dijk", Email = "isabel.vandijk@example.com" },
                new User { Id = "7ad3e8b6-22cc-498f-8c93-82a5c8b0a22b", Name = "Ruben Jansen", Email = "ruben.jansen@example.com" },
                new User { Id = "82f1b6c4-f6bb-4d92-a7df-94b8f87e6c44", Name = "Emma Smit", Email = "emma.smit@student.uva.nl" },
                new User { Id = "93c0e723-d9c2-4a8d-bbcf-35e2d3a5879b", Name = "Daan Kuiper", Email = "daan.kuiper@example.com" },
                new User { Id = "a4b6e2f8-7d6f-4e9b-b842-8a2c5f3b8d9e", Name = "Noah Visser", Email = "noah.visser@student.uva.nl" },
                new User { Id = "b5d7a8f9-3c5e-4f1d-a4e8-7d6c5b8e9a2f", Name = "Sophie Willems", Email = "sophie.willems@example.com" },
                new User { Id = "c6e8a9f2-5b7d-4c3f-a2e8-6f9d5c3b7a4e", Name = "Lucas Hermans", Email = "lucas.hermans@example.com" },
                new User { Id = "d7f9b0a3-6c5d-4e2b-a3d7-5b9e6c8f2a4e", Name = "Eva Peters", Email = "eva.peters@student.uva.nl" },
                new User { Id = "e8a0b1c4-7d5f-4c3b-a2d9-4e7f6c5b8a3e", Name = "Tom van den Berg", Email = "tom.vandenberg@example.com" },
                new User { Id = "f9b2c3d5-8a4e-4f3c-a2e7-6d5b9f7c8a1e", Name = "Sanne Bakker", Email = "sanne.bakker@student.uva.nl" },
                new User { Id = "0a1b2c3d-9e5f-4d3c-a2b7-6c8f5b7a9d4e", Name = "Milan de Jong", Email = "milan.dejong@example.com" },
                new User { Id = "1b2c3d4e-0a5f-4e3c-a2b9-7c6d5f8a7b9e", Name = "Yara Brouwer", Email = "yara.brouwer@student.uva.nl" }
            };


            // Teacher names
            var teachers = new List<User>
            {
                new User { Id = "3b83a493-eb84-4b32-b5cd-c79a3367eeec", Name = "Nicola Santolini", Email = "nicola.santolini@student.uva.nl" },
                new User { Id = "8c9dd623-f5de-46a2-bf91-df9b158906a6", Name = "Joachim Dekker", Email = "joachim.dekker@example.com" },
                new User { Id = "6e43a492-b34e-4b3a-bd72-d2f84d88537f", Name = "Karlo Peranovic", Email = "karlo.peranovic@student.uva.nl" }
            };

            // All students
            var studentUsers = realUsers.Where(u => !teachers.Any(t => t.Id == u.Id)).ToList();

            // Create users in the system
            var userIds = new List<string>();
            foreach (var user in teachers.Concat(studentUsers)) // Teachers followed by students
            {
                if (teachers.Contains(user)) // Teacher
                {
                    var teacher = await _userService.CreateTeacher(user.Id, user.Name, user.Email);
                    userIds.Add(teacher.Id);
                }
                else // Student
                {
                    var student = await _userService.CreateStudent(user.Id, user.Name, user.Email);
                    userIds.Add(student.Id);
                }
            }

            // Create courses for each teacher and assign students to them
            var courseIds = new List<string>();
            foreach (var teacher in teachers)
            {
                for (int i = 0; i < 3; i++) // Each teacher teaches 3 courses
                {
                    var courseId = Guid.NewGuid().ToString();
                    string courseName = Courses[new Random().Next(Courses.Count)].CourseName;
                    string courseDepartment = Departments[new Random().Next(Departments.Count)];

                    // Add the teacher as the course instructor
                    var courseTeachers = new List<string> { teacher.Id };
                    courseIds.Add(courseId);

                    // Create the course and assign students
                    await _courseService.CreateNewCourse(courseId, courseName, courseDepartment, courseTeachers);

                    // Add all students to the course
                    foreach (var studentId in userIds.Where(id => !courseTeachers.Contains(id)))
                    {
                        await _courseService.EnrollUser(courseId, studentId);
                    }
                }
            }

            // Generate sessions (5 sessions per course, starting from tomorrow, one per day)
            for (int i = 0; i < 5; i++) // 5 sessions for each course
            {
                var courseId = courseIds[new Random().Next(courseIds.Count)];
                var course = await _courseService.GetCourse(courseId);
                DateTime startTime = DateTime.Now.AddDays(i + 1); // Start from tomorrow
                DateTime endTime = startTime.AddHours(2);

                // Add all users (students and teachers) to the session
                await _attendanceService.CreateSession(courseId, startTime, endTime, userIds);
            }
        }

    }

}
