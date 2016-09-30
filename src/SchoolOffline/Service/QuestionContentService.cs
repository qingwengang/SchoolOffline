using Dapper;
using DoctorOffline.Service;
using MySql.Data.MySqlClient;
using SchoolOffline.Configs;
using SchoolOffline.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Service
{
    public class QuestionContentService : BaseSchoolService
    {
        public bool Add(QuestionContent content)
        {
            bool result = false;
            string sqlCommandText = @"INSERT INTO QuestionContent(RootId,PageId,Content,PageCount)VALUES(
                                    @RootId,
                                    @PageId,
                                    @Content,
                                    @PageCount)";
            using (IDbConnection conn = GetConnection())
            {
                int resultEx = conn.Execute(sqlCommandText, content);
                if (resultEx > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public QuestionContent GetContent(long rootId,int pageId)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format("select * from QuestionContent where rootid={0} and pageId = {1}",rootId,pageId);
            var list= con.Query<QuestionContent>(sql).ToList<QuestionContent>();
            return list.FirstOrDefault();
        }
        public List<QuestionContent> QueryBySql(string sql)
        {
            MySqlConnection con = GetConnection();
            var muluList = con.Query<QuestionContent>(sql).ToList<QuestionContent>();
            return muluList;
        }
    }
}
