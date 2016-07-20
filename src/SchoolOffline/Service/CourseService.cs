using MySql.Data.MySqlClient;
using SchoolOffline.Entity;
using System;
using Dapper;
using DoctorOffline.Service;
using System.Linq;
using System.Collections.Generic;
using SchoolOffline.Models;

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
        public List<string> GetDistinctTypeName()
        {
            return GetDistinct("select DISTINCT typename as col  from course");
        }
        public int GetMaxSortNumByMuluName(string muluName)
        {
            int result = 0;
            string sql = String.Format("select ifnull(max(sortnum),0) as col from course where muluname='{0}'", muluName);
            var muluList = GetDistinct(sql);
            if(muluList!=null && muluList.Count > 0)
            {
                result = int.Parse(muluList[0]);
            }
            return result;
        }
        public List<CourseSortModel> GetCourseSortModelByMuluName(long muluId)
        {
            MySqlConnection con = GetConnection();
            string sql = String.Format(@"select course.title,course.SortNum,course.Id from course inner join mulu on mulu.MuluName=course.MuluName 
                            where mulu.id = {0} order by course.SortNum", muluId);
            return con.Query<CourseSortModel>(sql).ToList<CourseSortModel>();
        }
        public void Update(Course course)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format(@"update course set typename='{0}',MuluName='{1}',Title='{2}',Content='{3}',SortNum='{4}'
                                where id = {5}", course.TypeName, course.MuluName, course.Title, course.Content, course.SortNum,course.Id);
            con.Execute(sql);
        }
    }
}
