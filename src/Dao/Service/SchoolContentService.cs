using DoctorOffline.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DoctorOffline.Service
{
    public class SchoolContentService : BaseSchoolService
    {
        public SchoolContent GetByMuluId(long muluId)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("select * from schoolcontent where muluid={0}", muluId);
            var muluList = con.Query<SchoolContent>(sql).ToList<SchoolContent>();
            if(muluList!=null && muluList.Count > 0)
            {
                return muluList.FirstOrDefault();
            }
            return null;
        }
        public void Save(SchoolContent schoolContent)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("update schoolcontent set content='{0}',titles='{2}' where muluid={1}", schoolContent.Content, schoolContent.MuluId,schoolContent.Titles);
            con.Execute(sql);
        }
        public void Add(SchoolContent content)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format(@"INSERT into schoolcontent (muluid,outcontent,content,titles)
                                values ({0},'{1}','{2}','{3}')", content.MuluId, content.OutContent, content.Content, content.Titles);
            con.Execute(sql);
        }
    }
}
