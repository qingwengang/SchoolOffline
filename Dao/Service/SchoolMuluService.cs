﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using DoctorOffline.Entity;

namespace DoctorOffline.Service
{
    public class SchoolMuluService : BaseSchoolService
    {
        public List<SchoolMulu> GetAll()
        {
            MySqlConnection con = GetConnection();
            String sql = "select * from schoolMulu";
            var muluList = con.Query<SchoolMulu>(sql).ToList<SchoolMulu>();
            return muluList;
        }

        public SchoolMulu GetByMuluId(long muluId)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("select * from schoolMulu where id={0}",muluId);
            var muluList = con.Query<SchoolMulu>(sql).ToList<SchoolMulu>();
            if(muluList!=null && muluList.Count > 0)
            {
                return muluList.FirstOrDefault();
            }
            return null;
        }
        public void Update(SchoolMulu mulu)
        {
            MySqlConnection con = GetConnection();
            String sql = String.Format("update schoolmulu set ifpassed={0} where id={1}", mulu.IfPassed, mulu.Id);
            con.Execute(sql);
        }
        public long Add(SchoolMulu mulu)
        {
            MySqlConnection con = GetConnection();
            String sql = string.Format(@"insert into schoolmulu (name,sort,outurl,type1,type2,spiderflag,ifpassed)VALUES
                                    ('{0}', 0, '', '{1}', '', 1, 0)", mulu.Name, mulu.Type1);
            con.Execute(sql);
            string maxIdString = QueryOne("select max(id) as col from schoolmulu");
            long result = 0;
            long.TryParse(maxIdString, out result);
            return result;
        }
    }
}
