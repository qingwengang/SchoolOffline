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
    public class MuluExtendService : BaseSchoolService
    {
        public void Add(MuluExtend muluExtend)
        {
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into MuluExtend (typename,relationtype,content)
                                    values('{0}','{1}','{2}')", muluExtend.TypeName,muluExtend.RelationType,muluExtend.Content));
        }
        public MuluExtend Get(string typeName,string relationType)
        {
            MySqlConnection con = GetConnection();
            string sql = String.Format("select * from MuluExtend where typename='{0}' and relationtype='{1}'", typeName,relationType);
            var muluList = con.Query<MuluExtend>(sql).ToList<MuluExtend>();
            return muluList.FirstOrDefault();
        }
        public void Update(MuluExtend muluExtend)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format(@"update MuluExtend set content='{0}'
                                where id = {1}", muluExtend.Content,muluExtend.Id);
            con.Execute(sql);
        }
        public void UpdateOrAdd(MuluExtend muluExtend)
        {
            var extend = Get(muluExtend.TypeName, muluExtend.RelationType);
            if(extend!=null)
            {;
                extend.Content = muluExtend.Content;
                Update(extend);
            }else
            {
                Add(muluExtend);
            }
        }
    }
}
