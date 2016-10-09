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
    public class QuestionTypeRelationService : BaseSchoolService
    {
        public string GetTypeDesc(string type)
        {
            MySqlConnection con = GetConnection();
            String sql = string.Format("select * from QuestionTypeRelation where questiontype='{0}'", type) ;
            var descList = con.Query<QuestionTypeRelation>(sql).ToList<QuestionTypeRelation>();
            if(descList!=null && descList.Count > 0)
            {
                return descList.FirstOrDefault().QuestionTypeDesc;
            }
            return string.Empty;
        }
    }
}
