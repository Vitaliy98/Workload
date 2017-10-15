using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkloadProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WorkloadProject.Controllers
{
    public class HomeController : Controller
    {
        private WorkloadContext db;
        public HomeController(WorkloadContext context)
        {
            db = context;
        }
        [Authorize(Roles ="admin")]
        public IActionResult Index()
        {
            ViewBag.Subjects = db.Subjects.ToList();
            ViewBag.Teachers = db.Teachers.Include(x => x.position).ToList();
            ViewBag.TypeLessons = db.TypeLessons.ToList();
            ViewBag.Positions = db.Positions.ToList();
            IQueryable<Lesson> lessons = db.Lessons.Include(x => x.Subject).Include(x => x.TypeLesson).Include(x => x.Teacher);
            ViewBag.Journal = db.Journals.Include(x => x.TimeLesson).ToList();
            ViewBag.User = db.Teachers.FirstOrDefault(x => x.Email == User.Identity.Name);
            return View(lessons.ToList());
        }
        [Authorize(Roles = "admin")]
        public IActionResult Teachers()
        {
                ViewBag.Positions = db.Positions.ToList();
                ViewBag.Teachers = db.Teachers.Include(x => x.position).ToList();
                return View();
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult Journal()
        {
                ViewBag.Journal = db.Journals.Include(x => x.Lesson).Include(x => x.Lesson.Subject).Include(x => x.Lesson.Teacher).
                    Include(x => x.Lesson.TypeLesson).Include(x => x.TimeLesson);
                return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateLesson(int teacherId, int subjectId, int typeLessonId, int hourLoad)
        { 
            db.Lessons.Add(new Lesson { Subject = db.Subjects.FirstOrDefault(x => x.Id == subjectId),
                TypeLesson = db.TypeLessons.FirstOrDefault(x => x.Id == typeLessonId),Teacher = db.Teachers.FirstOrDefault(x => x.Id == teacherId),
                HourLoad = hourLoad});

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLesson(int teacherId, int subjectId, int typeLessonId)
        {
           Lesson lesson = db.Lessons.FirstOrDefault(p => p.Teacher.Id == teacherId && p.Subject.Id == subjectId && p.TypeLesson.Id == typeLessonId);
            if(lesson != null)
            {
                db.Lessons.Remove(lesson);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound("Message");
        }
        [HttpPost]
        public async Task<IActionResult> AddSubject(string subjectName)
        {
            db.Subjects.Add(new Subject { Name = subjectName });
            await db.SaveChangesAsync();
            return RedirectToAction("Subjects");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            Subject subject = db.Subjects.FirstOrDefault(p => p.Id == subjectId);
            if (subject != null)
            {
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
                return RedirectToAction("Subjects");
            }
            return NotFound("Message");
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(string surname, string firstName, string middleName, int positionId, string email, string password)
        {
            db.Teachers.Add(new Teacher { Surname = surname, FirstName = firstName, MiddleName = middleName, Email = email, Password = password,
            position = db.Positions.FirstOrDefault(x => x.Id == positionId)});
            await db.SaveChangesAsync();
            return RedirectToAction("Teachers");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int teacherId)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(p => p.Id == teacherId);
            if (teacher != null)
            {
                db.Teachers.Remove(teacher);
                await db.SaveChangesAsync();
                return RedirectToAction("Teachers");
            }
            return NotFound("Message");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeLesson(int lessonId,int teacherId, int hourLoad)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(p => p.Id == teacherId);
            Lesson lesson = db.Lessons.FirstOrDefault(p => p.Id == lessonId && p.Teacher.Id == teacherId);
            if (teacher != null && lesson != null)
            {
                lesson.HourLoad = hourLoad;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound("Message=== TeacherId==="+teacherId+" LessonId===="+lessonId);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUser(int teacherId, string password)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(p => p.Id == teacherId);
            if (teacher != null)
            {
                teacher.Password = password;
                await db.SaveChangesAsync();
                return RedirectToAction("Teachers");
            }
            return NotFound("Message");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string userRole;
            if (ModelState.IsValid)
            {
                Teacher teacher = await db.Teachers.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (teacher != null)
                {
                    if (teacher.IsAdmin == true)
                        userRole = "admin";
                    else
                        userRole = "user";

                    await Authenticate(model.Email,userRole);

                    if(userRole == "admin")
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("UserPage", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            
            return View(model);
        }
        private async Task Authenticate(string userName,string userRole)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                    };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Subjects()
        {
            ViewBag.Subjects = db.Subjects.ToList();
            return View();
        }
        [Authorize(Roles = "user")]
        public IActionResult UserPage()
        {
            Teacher teacher = db.Teachers.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.User = teacher;
            ViewBag.Subjects = db.Subjects.ToList();
            ViewBag.TypeLessons = db.TypeLessons.ToList();
            IQueryable<Lesson> lessons = db.Lessons.Include(x => x.Subject).Include(x => x.TypeLesson).Include(x => x.Teacher).Where(x => x.Teacher.Id == teacher.Id);

            return View(lessons.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AddExecLesson(int lessonId, string date, int lessonNumber, string theme)
        {
            db.Journals.Add(new Journal
            {
               Lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId),
               Date = DateTime.Parse(date),
               TimeLesson = db.TimeLessons.FirstOrDefault(x => x.Id == lessonNumber),
               Theme = theme,
            });
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId);
            int oldHoursWorked = lesson.HoursWorked;
            lesson.HoursWorked = oldHoursWorked + 2;
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeExecLesson(int lessonId, string date, int lessonNumber, string newTheme)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId);
            DateTime dateTime = DateTime.Parse(date);
            Journal journal = db.Journals.FirstOrDefault(x => x.Lesson.Id == lesson.Id && x.Date.Equals(dateTime) && x.TimeLesson.Id == lessonNumber);
            journal.Theme = newTheme;
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteExecLesson(int lessonId, string date, int lessonNumber)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId);
            DateTime dateTime = DateTime.Parse(date);
            Journal journal = db.Journals.FirstOrDefault(x => x.Lesson.Id == lesson.Id && x.Date.Equals(dateTime) && x.TimeLesson.Id == lessonNumber);
            db.Journals.Remove(journal);
            lesson.HoursWorked = lesson.HoursWorked - 2;
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage");
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
