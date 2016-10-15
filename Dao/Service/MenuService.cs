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
    public class MenuService : BaseSchoolService
    {
        public Menu GetMenuByTypeName(string typeName)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("select * from menu where typename='{0}'", typeName);
            var muluList = con.Query<Menu>(sql).ToList<Menu>();
            return muluList == null || muluList.Count == 0 ? null : muluList.FirstOrDefault();
        }
        public void Add(Menu menu)
        {
            MySqlConnection con = GetConnection();
            con.Execute(String.Format(@"insert into menu (typename,content) values('{0}','{1}')", menu.TypeName,menu.Content));
        }
        public void Update(Menu menu)
        {
            MySqlConnection con = GetConnection();
            string sql = String.Format("update menu set content='{0}' where typename='{1}'", menu.Content, menu.TypeName);
            con.Execute(sql);
        }
    }
}
