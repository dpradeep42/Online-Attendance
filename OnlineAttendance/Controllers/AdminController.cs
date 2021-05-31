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
        public ActionResult Index()
        {
            return View();
        }
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
        public ActionResult AddClass()
        {
            return View();
        }
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
        public ActionResult AddFaculty()
        {
            return View();
        }
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
                return RedirectToAction("Faculty");
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Something Went Wrong','warning');";
                return RedirectToAction("Faculty");
            }
        }
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
    }
}