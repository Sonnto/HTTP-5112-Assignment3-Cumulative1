using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext SchoolDb = new SchoolDbContext();

        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {
            //Goal: Connect to database
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();

            //Run an SQL command "SELECT * FROM teachers"
            string query = "SELECT * FROM teachers";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> Teachers = new List<Teacher>();

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherfname"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                decimal Salary = Convert.ToDecimal(Convert.ToString(ResultSet["salary"]));

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.HireDate = HireDate;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.Salary = Salary;

                Teachers.Add(NewTeacher);
            }

            //GO through result set
            //For each row in result set, add Teacher info to Teacher List
            
            Conn.Close();

            return Teachers;
        }
        /// <summary>
        /// Grabs teacher from database
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <returns>
        /// {teacherid:1, teachername:"anthony ho", datehired:"2020-01-01",
        /// employeenumber: T001, salary: $100</returns>

        [HttpGet]
        [Route("api/teacherdata/findteacher/{teacherid}")]

        public Teacher FindTeacher(int TeacherId)
        {
            //GOAL: Connect to database
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();

            //Run SQL command "SELECT * FROM teachers"
            string query = "SELECT * FROM teachers WHERE teacherid=" + TeacherId;
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Teacher SelectedTeacher = new Teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                SelectedTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.Salary = Convert.ToDecimal(Convert.ToString(ResultSet["salary"]));
            }

            Conn.Close();

            return SelectedTeacher;
        }
    }
}
