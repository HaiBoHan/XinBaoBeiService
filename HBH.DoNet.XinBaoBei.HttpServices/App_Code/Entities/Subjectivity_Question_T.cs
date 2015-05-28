using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Subjectivity_Question_T 的摘要说明
    /// </summary>
    public class Subjectivity_Question_T
    {
        public Subjectivity_Question_T()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        private string question_Title;
        //专家问题标题
        public string Question_Title
        {
            get { return question_Title; }
            set { question_Title = value; }
        }

        private string question_Title_Part1;
        //专家问题标题部分1
        public string Question_Title_Part1
        {
            get { return question_Title_Part1; }
            set { question_Title_Part1 = value; }
        }

        private string question_Title_Part2;
        //专家问题标题部分2
        public string Question_Title_Part2
        {
            get { return question_Title_Part2; }
            set { question_Title_Part2 = value; }
        }

        private string aboutAge;
        //年龄段
        public string AboutAge
        {
            get { return aboutAge; }
            set { aboutAge = value; }
        }

        private string keyWords;
        //关键字
        public string KeyWords
        {
            get { return keyWords; }
            set { keyWords = value; }
        }

        private string aboutAgeBegin;
        //时间段开始
        public string AboutAgeBegin
        {
            get { return aboutAgeBegin; }
            set { aboutAgeBegin = value; }
        }
        private string aboutAgeEnd;
        //时间段结束
        public string AboutAgeEnd
        {
            get { return aboutAgeEnd; }
            set { aboutAgeEnd = value; }
        }

        private string answer;
        //时间段结束
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }



        public static Subjectivity_Question_T GetSubjectivity_Question_TFromTable(DataRow row)
        {
            Subjectivity_Question_T question = new Subjectivity_Question_T();

            question.ID = UIHelper.GetString(row["id"]);
            question.Question_Title = UIHelper.GetString(row["question_Title"]);

            question.AboutAge = UIHelper.GetString(row["aboutAge"]);
            question.KeyWords = UIHelper.GetString(row["keywords"]);
            question.AboutAgeBegin = UIHelper.GetString(row["aboutAgeBegin"]);
            question.AboutAgeEnd = UIHelper.GetString(row["aboutAgeEnd"]);
            question.Answer = UIHelper.GetString(row["answer"]);

            return question;
        }

        public static List<Subjectivity_Question_T> GetSubjectivity_Question_TFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Subjectivity_Question_T> list = new List<Subjectivity_Question_T>();
                foreach (DataRow row in table.Rows)
                {
                    Subjectivity_Question_T question = GetSubjectivity_Question_TFromTable(row);

                    if (question != null)
                    {
                        list.Add(question);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Subjectivity_Question_T> GetSubjectivity_Question_TFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetSubjectivity_Question_TFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}