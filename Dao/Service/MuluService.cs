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
    public class MuluService : BaseSchoolService
    {
        public List<Mulu> GetAll()
        {
            MySqlConnection con = GetConnection();
            String sql = "select * from Mulu order by TypeName,SortNum";
            var muluList = con.Query<Mulu>(sql).ToList<Mulu>();
            return muluList;
        }
        public Mulu GetByMuluId(long muluId)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("select * from mulu where id={0}", muluId);
            var muluList = con.Query<Mulu>(sql).ToList<Mulu>();
            if (muluList != null && muluList.Count > 0)
            {
                return muluList.FirstOrDefault();
            }
            return null;
        }
        public void AddMulu(Mulu mulu)
        {
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into mulu (typename,muluname,sortnum) values('{0}','{1}',{2})",mulu.TypeName,mulu.MuluName,mulu.SortNum));
        }
        public List<string> GetDistinctTypeName()
        {
            return GetDistinct("select distinct TypeName as col from Mulu order by TypeName");
        }
    }
}
