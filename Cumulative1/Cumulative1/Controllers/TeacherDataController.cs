using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext SchoolDb = new SchoolDbContext();

        /// <summary>
        /// Grabs teacher from database. If search key is included we will match teacher name to the search key
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <example>
        /// GET api/teacherdata/listteachers - > {teacherid:1, teachername:"anthony ho", datehired:"2020-01-01",
        /// employeenumber: T001, salary: $100
        /// </example>
        /// <returns>
        /// {teacherid:1, teachername:"anthony ho", datehired:"2020-01-01",
        /// employeenumber: T001, salary: $100
        /// </returns>

        [HttpGet]
        [Route("api/teacherdata/listteachers/{SearchKey}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey)
        {
            //Goal: Connect to database
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();

            Debug.WriteLine("The search key is " + SearchKey);

            //Run an SQL command "SELECT * FROM teachers"
            string query = "SELECT * FROM teachers WHERE teacherfname LIKE @key OR teacherlname LIKE @key OR hiredate LIKE @key OR employeenumber LIKE @key OR salary LIKE @key ORDER BY teacherlname";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            Debug.WriteLine("The query is: " + SearchKey);


            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> Teachers = new List<Teacher>();

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal Salary = Convert.ToDecimal(Convert.ToString(ResultSet["salary"]));

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                Teachers.Add(NewTeacher);
            }

            //GO through result set
            //For each row in result set, add Teacher info to Teacher List
            
            Conn.Close();

            return Teachers;
        }
        /// <summary>
        /// Grabs teacher from database. If search key is included we will match teacher name to the search key
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <example>
        /// GET api/teacherdata/listteachers - > {teacherid:1, teachername:"anthony ho", datehired:"2020-01-01",
        /// employeenumber: T001, salary: $100
        /// </example>
        /// <returns>
        /// {teacherid:1, teachername:"anthony ho", datehired:"2020-01-01",
        /// employeenumber: T001, salary: $100
        /// </returns>

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

            cmd.Parameters.AddWithValue("@id", TeacherId);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Teacher SelectedTeacher = new Teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                SelectedTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.Salary = Convert.ToDecimal(Convert.ToString(ResultSet["salary"]));
            }

            Conn.Close();

            return SelectedTeacher;
        }

        /// <summary>
        /// Adds new teacher to the system.
        /// </summary>
        /// <param name="NewTeacher">Teacher Object</param>
        /// <returns></returns>
        /// <example>
        /// api/TeacherData/AddTeacher
        /// </example>
        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //Query
            //INSERT into teachers (teacherfname, teacherlname, hiredate, employeenumber, salary) values (@fname, @lname, @hiredate, @employeenumber, @salary)
            string query = "INSERT into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES (@fname, @lname, @employeenumber, @hiredate, @salary)";

            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();


            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@fname", NewTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@lname", NewTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Method will delete a teacher from the database
        /// </summary>
        /// <param name="TeacherId">The teacher id primary key</param>
        /// <returns></returns>
        /// <example>
        /// GET: api/teacherdata/deleteteacher/100
        /// </example>

        [HttpGet]
        [Route("api/TeacherId/DeleteTeacher/{TeacherId}")]
        public void DeleteTeacher(int TeacherId)
        {
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();

            //Query
            //DELETE from teachers WHERE teacherid=@id
            string query = "DELETE from teachers WHERE TeacherId=@Id";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", TeacherId);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        /// <summary>
        /// Updates a teacher record in the system
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <param name="TeacherFName"></param>
        /// <param name="TeacherLName"></param>
        /// <param name="EmployeeNumber"></param>
        /// <param name="HireDate"></param>
        /// <param name="Salary"></param>
        /// <example>
        /// POST: /api/teacherdata/updateteacher/{teacherid}
        /// REQUEST BODY / POST DATA
        /// </example>
        [HttpPost]
        [Route("api/teacherdata/updateteacher/{teacherid}")]
        public void UpdateTeacher(int TeacherId,[FromBody]Teacher UpdatedTeacher)
        {
            Debug.WriteLine("Teacher ID is " + TeacherId);
            Debug.WriteLine("Teacher First Name is " + UpdatedTeacher.TeacherFName);
            Debug.WriteLine("Teacher Last Name is " + UpdatedTeacher.TeacherLName);
            Debug.WriteLine("Teacher Employee Number is " + UpdatedTeacher.EmployeeNumber);
            Debug.WriteLine("Teacher Hire Date is " + UpdatedTeacher.HireDate);
            Debug.WriteLine("Teacher Salary is " + UpdatedTeacher.Salary);

            //Update the Teacher Info Records
            string query = "UPDATE teachers SET TeacherFName=@fname, TeacherLName=@lname, EmployeeNumber=@employeenumber, HireDate=@hiredate, Salary=@salary WHERE TeacherId=@Id";
            MySqlConnection Conn = SchoolDb.AccessDatabase();
            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@fname", UpdatedTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@lname", UpdatedTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@employeenumber", UpdatedTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@hiredate", UpdatedTeacher.HireDate);
            cmd.Parameters.AddWithValue("@salary", UpdatedTeacher.Salary);
            cmd.Parameters.AddWithValue("@id", TeacherId);

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

    }
}
