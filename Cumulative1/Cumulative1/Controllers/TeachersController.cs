using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult List(string SearchKey)
        {
            Debug.WriteLine("The user is trying to search for " + SearchKey);

            //Need data from teacher data controller
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<Teacher> Teachers = MyController.ListTeachers(SearchKey);
            Debug.WriteLine("I have accessed " + SearchKey);
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