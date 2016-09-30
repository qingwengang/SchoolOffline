using Dapper;
using DoctorOffline.Service;
using MySql.Data.MySqlClient;
using SchoolOffline.Entity;
using SchoolOffline.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolOffline.Service
{
    public class QuestionService : BaseSchoolService
    {
        private int pageSize = 1;
        public long Add(Question question)
        {
            long id = 0;
            try
            {
                MySqlConnection con = GetConnection();
                con.Execute(String.Format(@"insert into question (type,title,des,createtime)
                                    values('{0}','{1}','{2}','{3}')", question.Type, question.Title, question.Des, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                id = GetMaxId();
            }
            catch (Exception e)
            {
                id = 0;
            }
            return id;
        }

        public long GetMaxId()
        {
            string idString = QueryOne("select max(id) as col from question");
            long maxId = long.Parse(idString);
            return maxId;
        }
        public List<Question> QueryBySql(string sql)
        {
            MySqlConnection con = GetConnection();
            var muluList = con.Query<Question>(sql).ToList<Question>();
            return muluList;
        }
        public QuestionPageModel GetQuestionPage(string type,int pageNo)
        {
            MySqlConnection con = GetConnection();
            string sql = string.Format("select * from question where type='{0}' order by createtime desc limit {1},{2}", type, (pageNo - 1) * pageSize, pageSize);
            var questionList = con.Query<Question>(sql).ToList<Question>();
            string getCount = string.Format("select count(id) as col from question where type='{0}'", type);
            string countString = QueryOne(getCount);
            int count = int.Parse(countString);
            QuestionPageModel pageDo = new QuestionPageModel
            {
                currentPage = pageNo,
                questionList = questionList,
                pageCount = (count+pageSize-1)/pageSize
            };
            return pageDo;
        }
    }
}
