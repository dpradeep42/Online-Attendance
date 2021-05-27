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
            return View();
        }
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
    }
}