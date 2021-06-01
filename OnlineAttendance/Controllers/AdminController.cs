using OnlineAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAttendance.Controllers
{
    public class AdminController : Controller
    {
        private readonly AttendanceEntities2 db = new AttendanceEntities2();
        // GET: Admin
        [AdminCheck]
        public ActionResult Index()
        {
            return View();
        }
        [AdminCheck]
        public ActionResult Faculty()
        {
            try
            {
                faculty f = new faculty();
                f.faculties = db.faculties.ToList();
                return View(f);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Index");
            }
        }
        [AdminCheck]
        public ActionResult Class()
        {
            try
            {
                zclass z = new zclass();
                z.zclasses = db.zclasses.ToList();
                return View(z);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Index");
            }
        }
        [AdminCheck]
        public ActionResult AddClass()
        {
            return View();
        }
        [AdminCheck]
        [HttpPost]
        public ActionResult AddClass(zclass zcl)
        {
            try
            {
                db.zclasses.Add(zcl);
                db.SaveChanges();
                return RedirectToAction("Class");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Class");
            }
        }
        [AdminCheck]
        public ActionResult EditClass(int id)
        {
            try
            {
                zclass existingClass = db.zclasses.Where(temp => temp.cid == id).FirstOrDefault();
                return View(existingClass);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return View("Class");
            }
        }
        [AdminCheck]
        [HttpPost]
        public ActionResult EditClass(zclass zcl)
        {
            try
            {
                zclass zc = db.zclasses.Where(temp => temp.cid == zcl.cid).FirstOrDefault();
                zc.classname = zcl.classname;
                db.SaveChanges();
                return RedirectToAction("Class");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return View("Class");
            }
        }
        [AdminCheck]
        public ActionResult Delete(long id)
        {
            try
            {
                zclass zc = db.zclasses.Where(temp => temp.cid == id).FirstOrDefault();
                db.zclasses.Remove(zc);
                db.SaveChanges();
                return RedirectToAction("Class");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Class");
            }
        }
        [AdminCheck]
        public ActionResult AddFaculty()
        {
            return View();
        }
        [AdminCheck]
        [HttpPost]
        public ActionResult AddFaculty(faculty fa)
        {
            try
            {
                db.faculties.Add(fa);
                db.SaveChanges();
                login l = new login
                {
                    username = fa.username,
                    password = fa.password,
                    type = fa.type,
                };
                db.logins.Add(l);
                db.SaveChanges();

                GMailer mailer = new GMailer();
                mailer.ToEmail = fa.email;
                mailer.Subject = "Account Created Successfully";
                mailer.Body = $"Dear Sir/Ma'am, Your Account created successfully!. Your credentials:. Username : {fa.username} Password:{fa.password}";
                mailer.IsHtml = false;
                mailer.Send();

                return RedirectToAction("Faculty");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Faculty");
            }
        }
        [AdminCheck]
        public ActionResult EditFaculty(int id)
        {
            try
            {
                faculty existingFaculty = db.faculties.Where(temp => temp.fid == id).FirstOrDefault();
                login l = db.logins.Where(ltemp => ltemp.username == existingFaculty.username).FirstOrDefault();
                db.logins.Remove(l);
                db.SaveChanges();
                return View(existingFaculty);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return View("Faculty");
            }
        }
        [AdminCheck]
        [HttpPost]
        public ActionResult EditFaculty(faculty fac)
        {
            try
            {
                faculty f = db.faculties.Where(temp => temp.fid == fac.fid).FirstOrDefault();
                f.name = fac.name;
                f.gender = fac.gender;
                f.mobile = fac.mobile;
                f.email = fac.email;
                f.username = fac.username;
                f.password = fac.password;
                f.subject = fac.subject;
                login l = new login
                {
                    username = fac.username,
                    password = fac.password,
                    type = fac.type,
                };
                db.logins.Add(l);
                db.SaveChanges();

                GMailer mailer = new GMailer();
                mailer.ToEmail = f.email;
                mailer.Subject = "Account Updated Successfully";
                mailer.Body = $"Dear Sir/Ma'am, Your Account updated successfully!. Your new credentials:. Username : {f.username} Password:{f.password}";
                mailer.IsHtml = false;
                mailer.Send();

                return RedirectToAction("Faculty");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return View("Faculty");
            }
        }
        [AdminCheck]
        public ActionResult DeleteFaculty(long id)
        {
            try
            {
                faculty fa = db.faculties.Where(temp => temp.fid == id).FirstOrDefault();
                login lo = db.logins.Where(l => l.username == fa.username).FirstOrDefault();
                db.logins.Remove(lo);
                db.faculties.Remove(fa);
                db.SaveChanges();

                return RedirectToAction("Faculty");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Faculty");
            }
        }
        [AdminCheck]
        public ActionResult Student()
        {
            try
            {
                student s = new student();
                s.students = db.students.ToList();
                return View(s);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Index");
            }
        }
        [AdminCheck]
        public ActionResult AddStudent()
        {
            try
            {
                student std = new student();
                std.zclasses = db.zclasses.ToList();
                return View(std);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Student");
            }
        }
        [AdminCheck]
        [HttpPost]
        public ActionResult AddStudent(student st)
        {
            try
            {
                db.students.Add(st);
                db.SaveChanges();
                login l = new login
                {
                    username = st.username,
                    password = st.password,
                    type = st.type,
                };
                db.logins.Add(l);
                db.SaveChanges();

                GMailer mailer = new GMailer();
                mailer.ToEmail = st.email;
                mailer.Subject = "Account Created Successfully";
                mailer.Body = $"Dear Sir/Ma'am, Your child's Account created successfully!. Your credentials:. Username : {st.username} Password:{st.password}";
                mailer.IsHtml = false;
                mailer.Send();

                return RedirectToAction("Student");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Student");
            }
        }
        [AdminCheck]
        public ActionResult DeleteStudent(long id)
        {
            try
            {
                student st = db.students.Where(temp => temp.sid == id).FirstOrDefault();
                login lo = db.logins.Where(l => l.username == st.username).FirstOrDefault();
                db.logins.Remove(lo);
                db.students.Remove(st);
                db.SaveChanges();

                return RedirectToAction("Student");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Student");
            }
        }
        [AdminCheck]
        public JsonResult CheckUsernameAvailability(string userdata)
        {
            //System.Threading.Thread.Sleep(200);
            var SeachData = db.logins.Where(x => x.username == userdata).FirstOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}