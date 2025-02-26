using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;

namespace AttendanceSystem.Domain.Services.Tools
{
    public class MockDataGenerator
    {
        private readonly UserService _userService;
        private readonly CourseService _courseService;
        private readonly AttendanceService _attendanceService;

        public MockDataGenerator(UserService userService, CourseService courseService, AttendanceService attendanceService)
        {
            _userService = userService;
            _courseService = courseService;
            _attendanceService = attendanceService;
        }

        // Name and surname arrays for random selection
        private static readonly List<string> FirstNames = new List<string>
    {
        "Jan", "Pieter", "Klaas", "Jeroen", "Sophie", "Anna", "Emma", "Luca", "Max", "Nina",
        "Lars", "Noah", "Sara", "Laura", "Tijs", "Robin", "Eva", "Julia", "Emma", "Sam",
        "Bram", "Koen", "Maarten", "Olivia", "Tom", "Liam", "Tessa", "Milan", "Sanne", "Daan",
        "Yara", "Zoë", "Frederik", "Maya", "Isa", "Joris", "Maxime", "Carmen", "Loes", "Iris"
    };

        private static readonly List<string> LastNames = new List<string>
    {
        "Jansen", "De Vries", "Bakker", "Visser", "Smit", "Meijer", "Hermans", "Van den Berg",
        "Janssen", "Willems", "Van Dijk", "Kuiper", "Brouwer", "Hendriks", "Mol", "Van Leeuwen",
        "De Boer", "Van der Meer", "Peters", "Bos", "Schouten", "Van Dam", "Huisman", "Evers",
        "Timmermans", "Van Wijk", "Vos", "De Jong", "Koster", "Van Ginkel", "Schoonhoven", "De Lange"
    };

        private static readonly List<string> TeacherNames = new List<string>
    {
        "Dr. John Adams", "Prof. Linda van den Berg", "Dr. Maria Garcia", "Prof. Michael Schmidt",
        "Dr. Thomas Baker", "Dr. Eva Jansen", "Dr. Emily Richards", "Prof. Tim Peters", "Dr. Anna Dijkstra",
        "Prof. Peter Smit", "Dr. Sam De Boer", "Prof. Clara Mulder", "Dr. Luke Hansen", "Prof. Elsa Janssen",
        "Dr. Simon Meijer", "Prof. Rachel Thomas", "Dr. Lisa Van Dijk", "Prof. Ian Sutherland", "Dr. Kate Miller",
        "Prof. James Brown", "Dr. Ruth Johnson", "Prof. Alexander Lee", "Dr. Sophia Evans", "Prof. Andrew Wilson",
        "Dr. Robert Moore", "Prof. Nancy King", "Dr. Joseph Hall", "Prof. Patricia Green", "Dr. Michael Clark"
    };

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

        public async Task GenerateMockData()
        {
            // Generate random teachers
            var teacherIds = new List<string>();
            for (int i = 0; i < 10; i++) // Creating 10 teachers for the mock data
            {
                string teacherName = $"{TeacherNames[new Random().Next(TeacherNames.Count)]}";
                string teacherEmail = $"{teacherName.Replace(" ", "").ToLower()}@example.com";
                var teacher = await _userService.CreateTeacher(Guid.NewGuid().ToString(), teacherName, teacherEmail);
                teacherIds.Add(teacher.Id);
            }

            // Generate random students
            var studentIds = new List<string>();
            for (int i = 0; i < 20; i++) // Creating 20 students
            {
                string studentName = $"{FirstNames[new Random().Next(FirstNames.Count)]} {LastNames[new Random().Next(LastNames.Count)]}";
                string studentEmail = $"{studentName.Replace(" ", "").ToLower()}@example.com";
                var student = await _userService.CreateStudent(Guid.NewGuid().ToString(), studentName, studentEmail);
                studentIds.Add(student.Id);
            }

            // Generate random courses
            var courseIds = new List<string>();
            foreach(var course in Courses)
            {

                //create a list of 2 random teachers id
                var courseTeachers = new List<string>();
                var random = new Random();
                for (int i = 0; i < 2; i++)
                {
                    string teacherId;
                    do
                    {
                        teacherId = teacherIds[random.Next(teacherIds.Count)];
                    } while (courseTeachers.Contains(teacherId));
                    courseTeachers.Add(teacherId);
                }
                string courseId = course.CourseId;
                string courseName = course.CourseName;
                string courseDepartment = "Department";
                var newCourse = new Course(courseId, courseName, courseDepartment, courseTeachers);
                courseIds.Add(courseId);
                await _courseService.CreateNewCourse(courseId, courseName, courseDepartment, courseTeachers);
            }

            // Generate random sessions and attendance
            for (int i = 0; i < 5; i++) // Creating 5 sessions
            {
                var courseId = courseIds[new Random().Next(courseIds.Count)];
                var course = await _courseService.GetCourse(courseId);
                DateTime startTime = DateTime.Now.AddDays(new Random().Next(1, 30));
                DateTime endTime = startTime.AddHours(2);

                var session = await _attendanceService.CreateSession(courseId, startTime, endTime);
            }
        }
    }

}
