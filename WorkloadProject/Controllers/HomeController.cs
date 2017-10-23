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
        public async Task<IActionResult> Index(string surname, int page = 1, SortState sortOrder = SortState.SurnameAsc)
        {
            ViewBag.Subjects = db.Subjects.ToList();
            ViewBag.TypeLessons = db.TypeLessons.ToList();
            ViewBag.Positions = db.Positions.ToList();
            IQueryable<Lesson> lessons = db.Lessons.Include(x => x.Subject).Include(x => x.TypeLesson).Include(x => x.Teacher);
            ViewBag.Journal = db.Journals.Include(x => x.TimeLesson).ToList();
            ViewBag.User = db.Teachers.FirstOrDefault(x => x.Email == User.Identity.Name);

            ViewBag.Lessons = db.Lessons.ToList();

            ViewBag.Teachers = db.Teachers.ToList();

            int pageSize = 5;

            //фильтрация
            IQueryable<Teacher> teachers = db.Teachers;

            if (!String.IsNullOrEmpty(surname))
            {
                teachers = teachers.Where(p => p.Surname.Contains(surname));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.SurnameDesc:
                    teachers = teachers.OrderByDescending(s => s.Surname);
                    break;
                default:
                    teachers = teachers.OrderBy(s => s.Surname);
                    break;
            }

            // пагинация
            var count = await teachers.CountAsync();
            var items = await teachers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(surname),
                Teachers = items
            };

            
            return View(viewModel);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Teachers(string surname, int page = 1, SortState sortOrder = SortState.SurnameAsc)
        {
                ViewBag.Positions = db.Positions.ToList();

            
            int pageSize = 5;

            //фильтрация
            IQueryable<Teacher> teachers = db.Teachers;

            if (!String.IsNullOrEmpty(surname))
            {
                teachers = teachers.Where(p => p.Surname.Contains(surname));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.FirstNameDesc:
                    teachers = teachers.OrderByDescending(s => s.FirstName);
                    break;
                case SortState.MiddleNameAsc:
                    teachers = teachers.OrderBy(s => s.MiddleName);
                    break;
                case SortState.MiddleNameDesc:
                    teachers = teachers.OrderByDescending(s => s.MiddleName);
                    break;
                case SortState.SurnameAsc:
                    teachers = teachers.OrderBy(s => s.Surname);
                    break;
                case SortState.SurnameDesc:
                    teachers = teachers.OrderByDescending(s => s.Surname);
                    break;
                case SortState.PositionAsc:
                    teachers = teachers.OrderBy(s => s.position.Name);
                    break;
                case SortState.PositionDesc:
                    teachers = teachers.OrderByDescending(s => s.position.Name);
                    break;
                default:
                    teachers = teachers.OrderBy(s => s.FirstName);
                    break;
            }

            // пагинация
            var count = await teachers.CountAsync();
            var items = await teachers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(surname),
                Teachers = items
            };


            return View(viewModel);

        }
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Journal(string surname, int page = 1, SortState sortOrder = SortState.SurnameAsc)
        {
            int pageSize = 5;

            //фильтрация
            IQueryable<Journal> journals = db.Journals.Include(x => x.Lesson).Include(x => x.Lesson.Subject).Include(x => x.Lesson.Teacher).
                    Include(x => x.Lesson.TypeLesson).Include(x => x.TimeLesson);

            if (!String.IsNullOrEmpty(surname))
            {
                journals = journals.Where(p => p.Lesson.Teacher.Surname.Contains(surname));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.SurnameDesc:
                    journals = journals.OrderByDescending(s => s.Lesson.Teacher.Surname);
                    break;
                case SortState.DateAsc:
                    journals = journals.OrderBy(s => s.Date);
                    break;
                case SortState.DateDesc:
                    journals = journals.OrderByDescending(s => s.Date);
                    break;
                case SortState.TimeAsc:
                    journals = journals.OrderBy(s => s.TimeLesson.Id);
                    break;
                case SortState.TimeDesc:
                    journals = journals.OrderByDescending(s => s.TimeLesson.Id);
                    break;
                case SortState.SubjectAsc:
                    journals = journals.OrderBy(s => s.Lesson.Subject.Name);
                    break;
                case SortState.SubjectDesc:
                    journals = journals.OrderByDescending(s => s.Lesson.Subject.Name);
                    break;
                default:
                    journals = journals.OrderBy(s => s.Lesson.Teacher.Surname);
                    break;
            }

            // пагинация
            var count = await journals.CountAsync();
            var items = await journals.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            JournalViewModel viewModel = new JournalViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(surname),
                Journals = items
            };

            return View(viewModel);
        }
        [Authorize(Roles = "user")]
        public async Task<IActionResult> JournalUser(string subjectName, int page = 1, SortState sortOrder = SortState.SubjectAsc)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.Lessons = db.Lessons.Include(x => x.Subject).Include(x => x.TypeLesson).Include(x => x.Teacher).Where(x => x.Teacher.Id == teacher.Id);
            int pageSize = 5;

            //фильтрация
            IQueryable<Journal> journals = db.Journals.Include(x => x.Lesson).Include(x => x.Lesson.Subject).Include(x => x.Lesson.Teacher).
                Include(x => x.Lesson.TypeLesson).Include(x => x.TimeLesson).Where(x => x.Lesson.Teacher.Id == teacher.Id);


            if (!String.IsNullOrEmpty(subjectName))
            {
                journals = journals.Where(p => p.Lesson.Subject.Name.Contains(subjectName));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.DateAsc:
                    journals = journals.OrderBy(s => s.Date);
                    break;
                case SortState.DateDesc:
                    journals = journals.OrderByDescending(s => s.Date);
                    break;
                case SortState.TimeAsc:
                    journals = journals.OrderBy(s => s.TimeLesson.Id);
                    break;
                case SortState.TimeDesc:
                    journals = journals.OrderByDescending(s => s.TimeLesson.Id);
                    break;
                case SortState.SubjectDesc:
                    journals = journals.OrderByDescending(s => s.Lesson.Subject.Name);
                    break;
                default:
                    journals = journals.OrderBy(s => s.Lesson.Subject.Name);
                    break;
            }

            // пагинация
            var count = await journals.CountAsync();
            var items = await journals.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            JournalUserViewModel viewModel = new JournalUserViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterSubjectsViewModel = new FilterSubjectsViewModel(subjectName),
                Journals = items
            };


            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
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

                    await Authenticate(model.Email, userRole);

                    if (userRole == "admin")
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("UserPage", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Subjects(string subjectName, int page = 1, SortState sortOrder = SortState.SubjectAsc)
        {

            int pageSize = 5;

            //фильтрация
            IQueryable<Subject> subjects = db.Subjects;

            if (!String.IsNullOrEmpty(subjectName))
            {
                subjects = subjects.Where(p => p.Name.Contains(subjectName));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.SubjectDesc:
                    subjects = subjects.OrderByDescending(s => s.Name);
                    break;
                default:
                    subjects = subjects.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await subjects.CountAsync();
            var items = await subjects.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubjectsViewModel viewModel = new SubjectsViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterSubjectsViewModel = new FilterSubjectsViewModel(subjectName),
                Subjects = items
            };


            return View(viewModel);

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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateLesson(int teacherId, int subjectId, int PractHourLoad, int LectHourLoad)
        {
            Lesson lesson = null;
            if (PractHourLoad != 0)
            {
                lesson = db.Lessons.FirstOrDefault(p => p.Subject.Id == subjectId && p.TypeLesson.Name == "Practice");
                if (lesson != null)
                {
                    return NotFound("Workload already exist");
                }
                else
                {
                    db.Lessons.Add(new Lesson
                    {
                        Subject = db.Subjects.FirstOrDefault(x => x.Id == subjectId),
                        TypeLesson = db.TypeLessons.FirstOrDefault(x => x.Name == "Practice"),
                        Teacher = db.Teachers.FirstOrDefault(x => x.Id == teacherId),
                        HourLoad = PractHourLoad
                    });

                    await db.SaveChangesAsync();
                }
            }
                if(LectHourLoad != 0)
                {

                    lesson = db.Lessons.FirstOrDefault(p => p.Subject.Id == subjectId && p.TypeLesson.Name == "Lecture");
                    if (lesson != null)
                    {
                        return NotFound("Workload already exist");
                    }
                    else
                    {
                        db.Lessons.Add(new Lesson
                        {
                            Subject = db.Subjects.FirstOrDefault(x => x.Id == subjectId),
                            TypeLesson = db.TypeLessons.FirstOrDefault(x => x.Name == "Lecture"),
                            Teacher = db.Teachers.FirstOrDefault(x => x.Id == teacherId),
                            HourLoad = LectHourLoad
                        });

                        await db.SaveChangesAsync();
                        
                    }
                }
                return RedirectToAction("Index");
            }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteLesson(int teacherId, int subjectId, int typeLessonId)
        {
           Lesson lesson = db.Lessons.FirstOrDefault(p => p.Teacher.Id == teacherId && p.Subject.Id == subjectId && p.TypeLesson.Id == typeLessonId);
            if(lesson != null)
            {
                db.Lessons.Remove(lesson);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound("Workload not found");
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddSubject(string subjectName)
        {
            Subject subject = db.Subjects.FirstOrDefault(p => p.Name == subjectName);
            if (subject != null)
            {
                return NotFound("Subject already exist");
            }
            else
            {
                db.Subjects.Add(new Subject { Name = subjectName });
                await db.SaveChangesAsync();
                return RedirectToAction("Subjects");
            }
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeSubject(int subjectId,string subjectName)
        {
            Subject subject = db.Subjects.FirstOrDefault(p => p.Id == subjectId);
            if (subject != null)
            {
                subject.Name = subjectName;
                await db.SaveChangesAsync();
                return RedirectToAction("Subjects");
            }
            else
            {
                return NotFound("Subject not found");
            }
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            Subject subject = db.Subjects.FirstOrDefault(p => p.Id == subjectId);
            if (subject != null)
            {
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
                return RedirectToAction("Subjects");
            }
            return NotFound("Subject not found");
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(string surname, string firstName, string middleName, int positionId, string email, string password)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(x => x.Surname == surname && x.FirstName == firstName && x.MiddleName == middleName);
            if (teacher != null)
            {
                return NotFound("Teacher already exist");
            }
            else
            {
                Position position = db.Positions.FirstOrDefault(x => x.Id == positionId);
                if (position != null)
                {
                    db.Teachers.Add(new Teacher
                    {
                        Surname = surname,
                        FirstName = firstName,
                        MiddleName = middleName,
                        Email = email,
                        Password = password,
                        position = db.Positions.FirstOrDefault(x => x.Id == positionId)
                    });
                    await db.SaveChangesAsync();
                    return RedirectToAction("Teachers");
                }
                else
                {
                    return NotFound("Position not found");
                }
            }
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int teacherId)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(p => p.Id == teacherId);
            if (teacher != null)
            {
                db.Teachers.Remove(teacher);
                await db.SaveChangesAsync();
                return RedirectToAction("Teachers");
            }
            return NotFound("Teacher not found");
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
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
            return NotFound("Workload not found");
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUser(string surname, string firstName, string middleName, int positionId, string email, string password, int teacherId)
        {
            Teacher currentTeacher = db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            Teacher teacher = db.Teachers.FirstOrDefault(x => x.Email.Equals(email) == true && x.FirstName.Equals(currentTeacher.FirstName) == false &&
                x.MiddleName.Equals(currentTeacher.MiddleName) == false && x.Surname.Equals(currentTeacher.Surname) == false);
            if (teacher != null)
            {
                return NotFound("Email is occupied by another teacher");
            }
            else
            {
                currentTeacher.Surname = surname;
                currentTeacher.FirstName = firstName;
                currentTeacher.MiddleName = middleName;
                currentTeacher.position = db.Positions.FirstOrDefault(x => x.Id == positionId);
                currentTeacher.Email = email;
                currentTeacher.Password = password;
                await db.SaveChangesAsync();
                return RedirectToAction("Teachers");
            }
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
        
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddExecLesson(int lessonId, string date, int lessonNumber, string theme)
        {
            TimeLesson timeLesson = db.TimeLessons.FirstOrDefault(x => x.Id == lessonNumber);
            if (timeLesson != null)
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
                return RedirectToAction("JournalUser");
            }
            else
            {
                return NotFound("Lesson Number not found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ChangeExecLesson(int lessonId, string date, int lessonNumber, string newTheme)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId);
            DateTime dateTime = DateTime.Parse(date);
            if (lesson == null)
            {
                return NotFound("Lesson not found");
            }
            else
            {
                Journal journal = db.Journals.FirstOrDefault(x => x.Lesson.Id == lesson.Id && x.Date.Equals(dateTime) && x.TimeLesson.Id == lessonNumber);
                if (journal != null)
                {
                    journal.Theme = newTheme;
                    await db.SaveChangesAsync();
                    return RedirectToAction("JournalUser");
                }
                else
                {
                    return NotFound("Journal not found");
                }
            }
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteExecLesson(int lessonId, string date, int lessonNumber)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == lessonId);
            DateTime dateTime = DateTime.Parse(date);
            TimeLesson timeLesson = db.TimeLessons.FirstOrDefault(x => x.Id == lessonNumber);
            if (timeLesson == null)
            {
                return NotFound("Lesson Number not found");
            }
            else
            {
                Journal journal = db.Journals.FirstOrDefault(x => x.Lesson.Id == lesson.Id && x.Date.Equals(dateTime) && x.TimeLesson.Id == lessonNumber);
                if (journal != null)
                {
                    db.Journals.Remove(journal);
                    lesson.HoursWorked = lesson.HoursWorked - 2;
                    await db.SaveChangesAsync();
                    return RedirectToAction("JournalUser");
                }
                else
                {
                    return NotFound("Journal not found");
                }
            }
        }
        [Authorize(Roles = "user")]
        public IActionResult ProfilUser()
        {
            ViewBag.User = db.Teachers.Include(x => x.position).FirstOrDefault(x => x.Email == User.Identity.Name);
            return View();
        }
    
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ChangeProfil(string surname, string firstName, string middleName, string email, string password)
        {
            Teacher currentTeacher = db.Teachers.FirstOrDefault(x => x.Email == User.Identity.Name);
            Teacher teacher = db.Teachers.FirstOrDefault(x => x.Email.Equals(email) == true && x.FirstName.Equals(currentTeacher.FirstName) == false && 
                x.MiddleName.Equals(currentTeacher.MiddleName) == false && x.Surname.Equals(currentTeacher.Surname) == false);
            if(teacher != null)
            {
                return NotFound("Email is occupied by another teacher");
            }
            else
            {
                currentTeacher.Surname = surname;
                currentTeacher.FirstName = firstName;
                currentTeacher.MiddleName = middleName;
                currentTeacher.Email = email;
                currentTeacher.Password = password;
                await db.SaveChangesAsync();
                return RedirectToAction("ProfilUser");
            }
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
