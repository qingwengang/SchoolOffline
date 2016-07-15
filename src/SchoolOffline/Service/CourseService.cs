using MySql.Data.MySqlClient;
using SchoolOffline.Entity;
using System;
using Dapper;
using DoctorOffline.Service;

namespace SchoolOffline.Service
{
    public class CourseService : BaseSchoolService
    {
        public void Add(Course course)
        {
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into course (typename,muluname,title,content,sortnum)
                                    values('{0}','{1}','{2}','{3}',{4})",course.TypeName,course.MuluName,course.Title,course.Content,course.SortNum));
        }
    }
}
