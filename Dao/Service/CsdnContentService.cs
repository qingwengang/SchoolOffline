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
    public class CsdnContentService : BaseSchoolService
    {
        public List<CsdnContent> GetContent(string type,int count)
        {
            string sql = String.Format("select * from csdncontent where type='{0}' and Flag is null or flag=0 LIMIT 0,{1}",type,count);
            MySqlConnection con = GetConnection();
            var muluList = con.Query<CsdnContent>(sql).ToList<CsdnContent>();
            return muluList;
        }
    }
}
