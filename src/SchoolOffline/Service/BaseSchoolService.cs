using MySql.Data.MySqlClient;
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
    }
}
