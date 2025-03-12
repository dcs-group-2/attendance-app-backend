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

        private static readonly List<(string CourseId, string CourseName)> Courses = new()
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


        private static readonly List<string> Departments = new()
        {
        "Computer Science", "Business Administration", "Philosophy", "Psychology", "Engineering", "Environmental Science",
        "Mathematics", "Marketing", "History", "Sociology", "Media Studies", "Economics", "Political Science", "Literature",
        "Education", "Law", "Health Sciences", "Arts", "Architecture", "Data Science", "Chemistry"
    };

        public async Task GenerateRealData()
        {
            // Real Users data from the CSV
            List<Student> realUsers =
            [
                new()
                {
                    Id = "0a615b34-a48e-4035-bba3-6cf07b883454", Name = "Paula Upil",
                    Email = "Paula.Upil@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "10c42de6-8144-44c8-ab33-64d9d86140a9", Name = "Alexander Serraris",
                    Email = "alexander.serraris@student.uva.nl"
                },
                new()
                {
                    Id = "14cd03f5-6f8f-49fd-a6ef-ecae6bcbcd82", Name = "Bo ter Ham",
                    Email = "Bo.TerHam@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "24989931-d191-4a45-96eb-15319b34aa53", Name = "Sam Tudent",
                    Email = "sam.tudent@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "27b0da69-2f96-48b1-872a-7ab1dc9ca441", Name = "Max Entee",
                    Email = "Max.Entee@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "2c120d5f-5a4a-4e74-8196-1a76af39dae6", Name = "Lieke Eerling",
                    Email = "Lieke.Eerling@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "37c81da2-872e-4d79-895b-1cfeabc3c005", Name = "Kekka Zone",
                    Email = "kekka.zone@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "413980e6-2cd7-4a77-b1b2-908bd01a4ccf", Name = "Alexi Dministrator",
                    Email = "alexi.dministrator@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "4e2091a4-53d0-4c5d-81d1-fa6287a42bc4", Name = "Beef Boss",
                    Email = "Beef.Boss@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "568efdc4-a887-4862-935d-235d9d25fa72", Name = "Doachim Jekker",
                    Email = "Doachim.Jekker@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "6270e6f2-8ccc-469a-bd06-da521c38d381", Name = "Georgia Raduate",
                    Email = "Georgia.Raduate@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "733226df-6eea-43a8-a5fd-9a805002eebb", Name = "Sicola Nantolini",
                    Email = "Sicola.Nantolini@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "9a5f09d1-3d84-4cc4-a9c9-8fbf6f0fd67e", Name = "Tim Rainee",
                    Email = "Tim.Rainee@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "b448c3c9-6341-414f-8f1c-9d1d77b66260", Name = "Serexander Alraris",
                    Email = "Serexander.Alraris@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "bce30295-8657-472f-aa8d-6cbcf47c0641", Name = "Parlo Keranovic",
                    Email = "Parlo.Keranovic@jdekker3008gmail.onmicrosoft.com"
                }
            ];


            // Teacher names
            var teachers = new List<Teacher>
            {
                new()
                {
                    Id = "50508f48-dbc9-4857-8caa-0a512ef49408", Name = "Tamara Eacher",
                    Email = "tamara.eacher@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "24e644af-efd4-411b-81c2-9e29523edfc2", Name = "Suki Einsei",
                    Email = "Suki.Ensei@jdekker3008gmail.onmicrosoft.com"
                },
                new()
                {
                    Id = "ab937eae-a79e-4c7f-9f33-d7728af5f5e8", Name = "Suesan Duva",
                    Email = "Suesan.Duva@jdekker3008gmail.onmicrosoft.com"
                },
            };
            
            // Admin users
            List<Administrator> admins =
            [
                new()
                {
                    Id = "01b17f54-f437-46c2-9a1d-3d18b08bedf7", Name = "Maurice Mouw",
                    Email = "maurice.mouw@sue.nl"
                },
                new()
                {
                    Id = "3b83a493-eb84-4b32-b5cd-c79a3367eeec", Name = "Nicola Santolini",
                    Email = "nicola.santolini@student.uva.nl"
                },
                new()
                {
                    Id = "3f359fb8-ae2e-49eb-9b36-3bba83cada3d", Name = "Karlo Peranović",
                    Email = "karlo.peranovic@student.uva.nl"
                },
                new()
                {
                    Id = "b0aec77f-3275-4d6f-b17b-f2c4a018fd99", Name = "Joachim Dekker",
                    Email = "jdekker3008@gmail.com"
                },
            ];
            
            // Add these users to the system, if they do not exist
            List<User> allUsers = [..realUsers,..admins, ..teachers];
            List<User> existingUsers = await _context.Users.ToListAsync();
            
            foreach (var user in allUsers.Where(user => existingUsers.All(u => u.Id != user.Id)))
            {
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();
            
            // Generate three courses
            List<Course> courses =
            [
                new Course("CS101", "Introduction to Programming", "Computer Science",
                    new List<string> { teachers[0].Id }),
                new Course("DSA101", "Data Structures and Algorithms", "Computer Science",
                    new List<string> { teachers[1].Id }),
                new Course("DMK101", "Digital Marketing", "Marketing", new List<string> { teachers[2].Id }),
                new Course("BI101", name: "Business Informatics", department: "Computer Science",
                    new List<string>() { teachers[1].Id, teachers[2].Id })
            ];
            
            // Add these courses to the system, if they do not exist
            List<Course> existingCourses = await _context.Courses.ToListAsync();
            foreach (var course in courses.Where(course => existingCourses.All(c => c.Id != course.Id)))
            {
                course.Students = realUsers.Select(u => u.Id).ToList();
                _context.Courses.Add(course);
                
                // Add sessions to the course
                for (int i = 0; i < 5; i++) // 5 sessions for each course
                {
                    DateTime startTime = DateTime.Now.AddDays(i + 1); // Start from tomorrow
                    DateTime endTime = startTime.AddHours(2);

                    Session session = new Session(course, realUsers.Select(u => u.Id).ToList(), startTime, endTime);
                    _context.Sessions.Add(session);
                    
                    course.Sessions.Add(session);
                }
            }
            
            await _context.SaveChangesAsync();
        }
    }

}
