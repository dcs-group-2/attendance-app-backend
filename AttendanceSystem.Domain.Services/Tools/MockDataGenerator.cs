using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services.Tools;

public class MockDataGenerator(CoursesContext context)
{
    public async Task GenerateRealData()
    {
        // Real Users data from the CSV
        Student samTudent = new()
        {
            Id = "24989931-d191-4a45-96eb-15319b34aa53", Name = "Sam Tudent",
            Email = "sam.tudent@jdekker3008gmail.onmicrosoft.com",
        };

        Student liekeEerling = new()
        {
            Id = "2c120d5f-5a4a-4e74-8196-1a76af39dae6", Name = "Lieke Eerling",
            Email = "Lieke.Eerling@jdekker3008gmail.onmicrosoft.com",
        };

        Student beefBoss = new()
        {
            Id = "4e2091a4-53d0-4c5d-81d1-fa6287a42bc4", Name = "Beef Boss",
            Email = "Beef.Boss@jdekker3008gmail.onmicrosoft.com",
        };

        List<Student> group1 =
        [
            new()
            {
                Id = "0a615b34-a48e-4035-bba3-6cf07b883454", Name = "Paula Upil",
                Email = "Paula.Upil@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "10c42de6-8144-44c8-ab33-64d9d86140a9", Name = "Alexander Serraris",
                Email = "alexander.serraris@student.uva.nl",
            },
            new()
            {
                Id = "14cd03f5-6f8f-49fd-a6ef-ecae6bcbcd82", Name = "Bo ter Ham",
                Email = "Bo.TerHam@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "27b0da69-2f96-48b1-872a-7ab1dc9ca441", Name = "Max Entee",
                Email = "Max.Entee@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "37c81da2-872e-4d79-895b-1cfeabc3c005", Name = "Kekka Zone",
                Email = "kekka.zone@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "568efdc4-a887-4862-935d-235d9d25fa72", Name = "Doachim Jekker",
                Email = "Doachim.Jekker@jdekker3008gmail.onmicrosoft.com",
            },
        ];

        List<Student> group2 =
        [
            new()
            {
                Id = "6270e6f2-8ccc-469a-bd06-da521c38d381", Name = "Georgia Raduate",
                Email = "Georgia.Raduate@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "733226df-6eea-43a8-a5fd-9a805002eebb", Name = "Sicola Nantolini",
                Email = "Sicola.Nantolini@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "9a5f09d1-3d84-4cc4-a9c9-8fbf6f0fd67e", Name = "Tim Rainee",
                Email = "Tim.Rainee@jdekker3008gmail.onmicrosoft.com",
            },
        ];

        List<Student> group3 =
        [
            new()
            {
                Id = "b448c3c9-6341-414f-8f1c-9d1d77b66260", Name = "Serexander Alraris",
                Email = "Serexander.Alraris@jdekker3008gmail.onmicrosoft.com",
            },
            new()
            {
                Id = "bce30295-8657-472f-aa8d-6cbcf47c0641", Name = "Parlo Keranovic",
                Email = "Parlo.Keranovic@jdekker3008gmail.onmicrosoft.com",
            },
        ];

        List<Student> students =
        [
            samTudent,
            liekeEerling,
            beefBoss,
            ..group1,
            ..group2,
            ..group3,
        ];


        // Teacher names
        List<Teacher> teachers =
        [
            new()
            {
                Id = "50508f48-dbc9-4857-8caa-0a512ef49408", Name = "Tamara Eacher",
                Email = "tamara.eacher@jdekker3008gmail.onmicrosoft.com",
            },

            new()
            {
                Id = "24e644af-efd4-411b-81c2-9e29523edfc2", Name = "Suki Einsei",
                Email = "Suki.Ensei@jdekker3008gmail.onmicrosoft.com",
            },

            new()
            {
                Id = "ab937eae-a79e-4c7f-9f33-d7728af5f5e8", Name = "Suesan Duva",
                Email = "Suesan.Duva@jdekker3008gmail.onmicrosoft.com",
            },
        ];

        // Admin users
        List<Administrator> admins =
        [
            new()
            {
                Id = "01b17f54-f437-46c2-9a1d-3d18b08bedf7", Name = "Maurice Mouw",
                Email = "maurice.mouw@sue.nl",
            },
            new()
            {
                Id = "3b83a493-eb84-4b32-b5cd-c79a3367eeec", Name = "Nicola Santolini",
                Email = "nicola.santolini@student.uva.nl",
            },
            new()
            {
                Id = "3f359fb8-ae2e-49eb-9b36-3bba83cada3d", Name = "Karlo Peranović",
                Email = "karlo.peranovic@student.uva.nl",
            },
            new()
            {
                Id = "b0aec77f-3275-4d6f-b17b-f2c4a018fd99", Name = "Joachim Dekker",
                Email = "jdekker3008@gmail.com",
            },
            new()
            {
                Id = "413980e6-2cd7-4a77-b1b2-908bd01a4ccf", Name = "Alexi Dministrator",
                Email = "alexi.dministrator@jdekker3008gmail.onmicrosoft.com",
            },
        ];

        // Add these users to the system if they do not exist
        List<User> allUsers = [..students, ..admins, ..teachers];
        List<User> existingUsers = await context.Users.ToListAsync();

        foreach (User user in allUsers.Where(user => existingUsers.All(u => u.Id != user.Id)))
        {
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();

        // Generate three courses
        Course cs101 = new("CS101",
            "Introduction to Programming",
            "Computer Science",
            new List<string> { teachers[0].Id }
        );
        Course dsa101 = new("DSA101",
            "Data Structures and Algorithms",
            "Computer Science",
            new List<string> { teachers[1].Id }
        );
        Course dmk101 = new("DMK101",
            "Digital Marketing",
            "Marketing",
            new List<string> { teachers[2].Id }
        );
        Course bi101 = new("BI101",
            "Business Informatics",
            "Computer Science",
            new List<string> { teachers[1].Id, teachers[2].Id }
        );

        List<Course> courses =
        [
            cs101, dsa101, dmk101, bi101,
        ];

        Dictionary<Course, List<Student>> courseStudents = new()
        {
            { cs101, [samTudent, beefBoss, ..group1, ..group2] },
            { dsa101, [beefBoss, samTudent, ..group1, ..group2, ..group3] },
            { dmk101, [liekeEerling, ..group3] },
            { bi101, [liekeEerling, beefBoss, ..group2, ..group3] },
        };

        // Add these courses to the system if they do not exist
        List<Course> existingCourses = await context.Courses.ToListAsync();
        DateTime startDate = new(2025, 4, 1, 9, 0, 0, DateTimeKind.Utc);
        foreach (Course course in courses.Where(course => existingCourses.All(c => c.Id != course.Id)))
        {
            course.Students = courseStudents[course].Select(u => u.Id).ToList();
            context.Courses.Add(course);

            // Add sessions to the course
            for (int i = 0; i < 5; i++) // 5 sessions for each course
            {
                DateTime startTime = startDate.AddHours(i * 2);
                DateTime endTime = startTime.AddHours((i + 1) * 2);

                Session session = new(course, course.Students, startTime, endTime);
                context.Sessions.Add(session);

                course.Sessions.Add(session);
            }
        }

        await context.SaveChangesAsync();
    }
}
