using OnlineAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAttendance.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly AttendanceEntities2 db = new AttendanceEntities2();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(login l)
        {
            try
            {
                login lg = db.logins.Where(temp => temp.username == l.username ).FirstOrDefault();
                if(lg != null)
                {
                    if(lg.password == l.password)
                    {
                        if(lg.type == "AD")
                        {
                            Session["AdminStart"] = "SessionOn";
                            TempData["notification"] = "swal('','Login Success','success');";
                            return RedirectToAction("../Admin/Index");
                        }
                        if (lg.type == "FA")
                        {
                            TempData["notification"] = "swal('','Login Success','success');";
                            return RedirectToAction("../Faculty/Index");
                        }
                        if (lg.type == "ST")
                        {
                            TempData["notification"] = "swal('','Login Success','success');";
                            return RedirectToAction("../Student/Index");
                        }
                    }
                    else
                    {
                        TempData["notification"] = "swal('','Invalid Username or Password','warning');";
                        goto GTIndex;
                    }
                }
                else
                {
                    TempData["notification"] = "swal('','Invalid Username or Password','warning');";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["notification"] = "swal('','Invalid Username or Password','warning');";
            }
        GTIndex:
            return RedirectToAction("Index");
        }
    }
}