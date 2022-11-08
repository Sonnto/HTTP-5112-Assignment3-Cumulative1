using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative1.Models;

namespace Cumulative1.Controllers
{
    public class TeachersController : Controller
    {
        // GET: /Teachers/List
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            //Need data from article data controller
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<Teacher> Teachers = MyController.ListTeachers();
            return View(Teachers);
        }

        //GET: /Teachers/Show/{teahcerid}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}