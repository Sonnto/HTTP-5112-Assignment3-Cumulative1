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

        //GET: /Teachers/Show/{teacherid}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //GET: /Teachers/New
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string TeacherFName, string TeacherLName, int EmployeeNumber, DateTime HireDate, int Salary)
        {
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFName = TeacherFName; 
            NewTeacher.TeacherLName = TeacherLName;
            NewTeacher.EmployeeNumber = EmployeeNumber.ToString();
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController MyController = new TeacherDataController();
            MyController.AddTeacher(NewTeacher);

            Debug.WriteLine("Trying to create a new teacher with first and last name " + TeacherFName + TeacherLName);
            return RedirectToAction("List");
        }

        //GET: /Teachers/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            //navigate to /Views/Teachers/DeleteConfirm.html
            TeacherDataController MyController = new TeacherDataController();
            Teacher NewTeacher = MyController.FindTeacher(id);
            ///MyController.DeleteTeacher(id);
            return View(NewTeacher);
        }

        [HttpPost]
        public ActionResult Delete (int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            MyController.DeleteTeacher(id);
            return RedirectToAction("List");
        }

    }
}