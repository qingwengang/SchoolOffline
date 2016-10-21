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
            con.Execute(String.Format(@"insert into CourseTitle (draftId,titleName,content,sortnum)
                                    values({0},'{1}','{2}',{3})", course.DraftId,course.TitleName,"",course.SortNum));
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
        public void Update(CourseTitle courseTitle)
        {
            string sql = string.Format("update coursetitle set titlename='{0}',content='{1}',sortnum={2} where id={3}", courseTitle.TitleName, courseTitle.Content, courseTitle.SortNum, courseTitle.Id);
            MySqlConnection con = GetConnection();
            con.Execute(sql);
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
