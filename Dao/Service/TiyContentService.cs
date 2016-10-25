using Dapper;
using DoctorOffline.Service;
using MySql.Data.MySqlClient;
using SchoolOffline.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Service
{
    public class TiyContentService:BaseSchoolService
    {
        public List<TiyContent> GetByCourseTitle(string type)
        {
            MySqlConnection con = GetConnection();
            var sql = string.Format("select * from tiycontent where type='{0}'", type);
            return con.Query<TiyContent>(sql).ToList<TiyContent>(); 
        }
        public List<TiyContent> GetAll()
        {
            MySqlConnection con = GetConnection();
            var sql = string.Format("select * from tiycontent");
            return con.Query<TiyContent>(sql).ToList<TiyContent>();
        }
        public TiyContent GetById(long id)
        {
            MySqlConnection con = GetConnection();
            var sql = string.Format("select * from tiycontent where id='{0}'", id);
            var tiyList = con.Query<TiyContent>(sql).ToList<TiyContent>();
            return tiyList != null && tiyList.Count > 0 ? tiyList[0] : null;
        }
        public string Add(TiyContent content)
        {
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into tiycontent(title,content,coursetitle,type,lastmod) VALUES('{0}','{1}','{2}','{3}',now())",content.Title,content.Content,content.CourseTitle,content.Type));
            string sql = "select max(id) as col from tiycontent";
            List<string> ls = GetDistinct(sql);
            if(ls!=null && ls.Count > 0)
            {
                return ls.FirstOrDefault();
            }
            return "";
        }
        public void Update(TiyContent content)
        {
            MySqlConnection con = GetConnection();
            var sql = string.Format("update tiycontent set title='{0}',CourseTitle='{1}',Content='{2}',type='{4}',lastmod=now() where id={3}", content.Title, content.CourseTitle, content.Content, content.Id,content.Type);
            con.Execute(sql);
        }
        public List<TiyContent> GetAllForSiteMap()
        {
            string sql = "select id,title,'' as content,coursetitle,'' as type,lastmod from tiycontent";
            MySqlConnection con = GetConnection();
            return con.Query<TiyContent>(sql).ToList<TiyContent>();
        }
    }
}
