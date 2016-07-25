using Dapper;
using MySql.Data.MySqlClient;
using SchoolOffline.Configs;
using SchoolOffline.Entity.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorOffline.Service
{
    public class BaseSchoolService
    {
        public MySqlConnection GetConnection()
        {
            MySqlConnection con = new MySqlConnection(OnlineConfig.dbUrl);
            return con;
        }

        public List<string> GetDistinct(string sql)
        {
            List<string> result = new List<string>();
            MySqlConnection con = GetConnection();
            var muluList = con.Query<DistinctDo>(sql).ToList<DistinctDo>();
            foreach(var s in muluList){
                result.Add(s.col);
            }
            return result;
        }
    }
}
