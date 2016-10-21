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
    public class CourseDraftService : BaseSchoolService
    {
        public void Add(CourseDraft course)
        {
            if (course.SortNum == 0)
            {
                course.SortNum = GetMaxSortNumByMuluName(course.MuluName) + 1;
            }
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into CourseDraft (typename,muluname,title,content,sortnum)
                                    values('{0}','{1}','{2}','{3}',{4})", course.TypeName, course.MuluName, course.Title, course.Content,course.SortNum));
        }
        public CourseDraft GetById(long draftid)
        {
            string sql = string.Format("select * from coursedraft where id={0}", draftid);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<CourseDraft>(sql).ToList<CourseDraft>();
            return muluList.FirstOrDefault();
        }
        public List<CourseDraft> GetByMuluName(string muluName)
        {
            string sql = string.Format("select * from CourseDraft where muluName='{0}' order by sortNum", muluName);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<CourseDraft>(sql).ToList<CourseDraft>();
            return muluList;
        }
        public void Update(CourseDraft course)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format(@"update CourseDraft set typename='{0}',MuluName='{1}',Title='{2}',Content='{3}',SortNum='{4}'
                                where id = {5}", course.TypeName, course.MuluName, course.Title, course.Content, course.SortNum, course.Id);
            con.Execute(sql);
        }
        public int GetMaxSortNumByMuluName(string muluName)
        {
            int result = 0;
            string sql = String.Format("select ifnull(max(sortnum),0) as col from coursedraft where muluname='{0}'", muluName);
            var muluList = GetDistinct(sql);
            if (muluList != null && muluList.Count > 0)
            {
                result = int.Parse(muluList[0]);
            }
            return result;
        }
    }
}
