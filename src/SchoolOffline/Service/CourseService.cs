using MySql.Data.MySqlClient;
using SchoolOffline.Entity;
using System;
using Dapper;
using DoctorOffline.Service;
using System.Linq;
using System.Collections.Generic;

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

        public Course GetById(long id)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("select * from course where id={0}", id);
            var muluList = con.Query<Course>(sql).ToList<Course>();
            if (muluList != null && muluList.Count > 0)
            {
                return muluList.FirstOrDefault();
            }
            return null;
        }
        public List<Course> GetCourseByTypeName(string typeName)
        {
            MySqlConnection con = GetConnection();
            string sql = String.Format("select title,course.MuluName,course.id,'' as TypeName,'' as Content,course.SortNum from course inner join mulu on course.MuluName=mulu.MuluName where mulu.typename='{0}'  order by mulu.SortNum,course.SortNum", typeName);
            var muluList = con.Query<Course>(sql).ToList<Course>();
            return muluList;
        }
    }
}
