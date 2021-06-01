using OnlineAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAttendance.Controllers
{
    public class FacultyController : Controller
    {
        private readonly AttendanceEntities2 db = new AttendanceEntities2();
        // GET: Faculty
        [FacultyCheck]
        public ActionResult Index()
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
                return RedirectToAction("Index","Home");
            }
        }
        [FacultyCheck]
        public ActionResult ViewClass(int id)
        {
            try
            {
                Session["ClassId"] = id;
                zclass zc = db.zclasses.Where(z => z.cid == id).FirstOrDefault();
                student st = new student();
                st.students = db.students.Where(temp => temp.classname == zc.classname).ToList();
                return View(st);
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Index");
            }
        }
        [FacultyCheck]
        public ActionResult Present(int id)
        {
            try
            {
                student s = db.students.Where(temp => temp.sid == id).FirstOrDefault();

                int fid = Convert.ToInt32(Session["fid"]);

                faculty f = db.faculties.Where(ftemp => ftemp.fid == fid).FirstOrDefault();

                GMailer mailer = new GMailer();
                mailer.ToEmail = s.email;
                mailer.Subject = $"{s.name} is Present for {f.subject} class today.";
                mailer.Body = $"Dear Sir/Ma'am, Your son/daughter is present for {f.subject} class today. For more information on your child performance and other details you can contact the respective faculty {f.name} on {f.mobile} or {f.email}";
                mailer.IsHtml = false;
                mailer.Send();

                attendance at = new attendance
                {
                    stid = s.username,
                    name = s.name,
                    date = DateTime.UtcNow.ToString("d"),
                    subject = f.subject,
                    markedBy = f.name,
                    status = "Present"
                };
                db.attendances.Add(at);
                db.SaveChanges();

                int cid = Convert.ToInt32(Session["ClassId"]);
                return RedirectToAction("ViewClass", "Faculty", new { id = cid });
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";

                int cid = Convert.ToInt32(Session["ClassId"]);

                zclass zc = db.zclasses.Where(z => z.cid == cid).FirstOrDefault();
                student st = new student();
                st.students = db.students.Where(temp => temp.classname == zc.classname).ToList();

                return RedirectToAction("ViewClass", "Faculty", new { id = cid });
            }
        }
        [FacultyCheck]
        public ActionResult Absent(int id)
        {
            try
            {
                student s = db.students.Where(temp => temp.sid == id).FirstOrDefault();

                int fid = Convert.ToInt32(Session["fid"]);

                faculty f = db.faculties.Where(ftemp => ftemp.fid == fid).FirstOrDefault();

                GMailer mailer = new GMailer();
                mailer.ToEmail = s.email;
                mailer.Subject = $"{s.name} is Absent for {f.subject} class today.";
                mailer.Body = $"Dear Sir/Ma'am, Your son/daughter is Absent for {f.subject} class today. For more information on your child performance and other details you can contact the respective faculty {f.name} on {f.mobile} or {f.email}";
                mailer.IsHtml = false;
                mailer.Send();

                attendance at = new attendance
                {
                    stid = s.username,
                    name = s.name,
                    date = DateTime.UtcNow.ToString("d"),
                    subject = f.subject,
                    markedBy = f.name,
                    status = "Absent"
                };
                db.attendances.Add(at);
                db.SaveChanges();

                int cid = Convert.ToInt32(Session["ClassId"]);
                return RedirectToAction("ViewClass", "Faculty", new { id = cid });
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";

                int cid = Convert.ToInt32(Session["ClassId"]);

                zclass zc = db.zclasses.Where(z => z.cid == cid).FirstOrDefault();
                student st = new student();
                st.students = db.students.Where(temp => temp.classname == zc.classname).ToList();

                return RedirectToAction("ViewClass", "Faculty", new { id = cid });
            }
        }

    }
}