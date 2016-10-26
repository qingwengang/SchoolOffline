using Dao.Entity;
using Dapper;
using DoctorOffline.Service;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dao.Service
{
    public class CourseTitleService : BaseSchoolService
    {
        public void Add(CourseTitle course)
        {
            course.SortNum = GetMaxSortNumByMuluName(course.DraftId)+1;
            MySqlConnection con = GetConnection();
            string sql = "insert into CourseTitle (draftId,titleName,content,sortnum,lastmod) values(@DraftId,@TitleName,'',@SortNum,now())";
            int result = con.Execute(sql, course);
        }

        public void Update(CourseTitle courseTitle)
        {
            string sql = "update coursetitle set titlename=@TitleName,content=@Content,sortnum=@SortNum,lastmod=now() where id=@Id";
            MySqlConnection con = GetConnection();
            int result = con.Execute(sql, courseTitle);
        }
        public List<CourseTitle> GetByDraftId(long draftId)
        {
            string sql = string.Format("select * from CourseTitle where draftId={0} order by sortnum", draftId);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<CourseTitle>(sql).ToList<CourseTitle>();
            return muluList;
        }
        public CourseTitle GetByTitleId(long titleId)
        {
            string sql = String.Format("select * from courseTitle where id={0}", titleId);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<CourseTitle>(sql).ToList<CourseTitle>();
            return muluList.FirstOrDefault();
        }
        
        public int GetMaxSortNumByMuluName(long draftId)
        {
            int result = 0;
            string sql = String.Format("select ifnull(max(sortnum),0) as col from CourseTitle where draftId='{0}'", draftId);
            var muluList = GetDistinct(sql);
            if (muluList != null && muluList.Count > 0)
            {
                result = int.Parse(muluList[0]);
            }
            return result;
        }
    }
}
