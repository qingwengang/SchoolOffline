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
            string sql = "insert into course (typename,muluname,title,content,sortnum,outerid,draftid,lastmod) values(@TypeName, @MuluName, @Title, @Content,@SortNum,@OuterId,@DraftId,now())";
            con.Execute(sql, course);
        }
        public void Update(Course course, bool ifUpdatetime = false)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Empty;
            if (ifUpdatetime)
            {
                sql = "update course set typename=@TypeName,MuluName=@MuluName,Title=@Title,Content=@Content,SortNum=@SortNum,comment=@Comment,lastmod=now() where id = @Id";
            }
            else
            {
                sql = "update course set typename=@TypeName,MuluName=@MuluName,Title=@Title,Content=@Content,comment=@Comment,SortNum=@SortNum where id = @Id";
            }
            con.Execute(sql,course);
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
        public List<Course> QueryBySql(string sql)
        {
            MySqlConnection con = GetConnection();
            var muluList = con.Query<Course>(sql).ToList<Course>();
            return muluList;
        }
        public Course GetMinCourseByType(string typeName)
        {
            string sql = string.Format("select * from course where typeName='{0}' order by sortnum limit 0,1", typeName);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<Course>(sql).ToList<Course>();
            return muluList.FirstOrDefault();
        }
        public List<Course> GetCourseByTypeName(string typeName)
        {
            MySqlConnection con = GetConnection();
            string sql = String.Format("select title,course.MuluName,course.id,'' as TypeName,'' as Content,course.SortNum,course.outerid,lastpage,nextpage from course inner join mulu on course.MuluName=mulu.MuluName where mulu.typename='{0}'  order by mulu.SortNum,course.SortNum", typeName);
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
        
        public void UpdatePageHref(Course course)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format(@"update course set lastpage='{0}',nextpage='{1}'
                                where id = {2}", course.LastPage,course.NextPage, course.Id);
            con.Execute(sql);
        }
        public List<CourseSortModel> GetBySql(string sql)
        {
            MySqlConnection con = GetConnection();
            return con.Query<CourseSortModel>(sql).ToList<CourseSortModel>();
        }

        public List<Course> GetAllForSiteMap()
        {
            string sql = "select id,typename,title,lastmod,'' as muluname,'' as content,0 as sortnum,0 as outerid,'' as lastpage,'' as nextpage,0 as draftid from course";
            return QueryBySql(sql);
        }
    }
}
