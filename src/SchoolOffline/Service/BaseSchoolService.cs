using Dapper;
using MySql.Data.MySqlClient;
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
            MySqlConnection con = new MySqlConnection("Data Source=127.0.0.1;Initial Catalog=schoolproduct;Persist Security Info=True;User ID=root;Password=ganggang");
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
