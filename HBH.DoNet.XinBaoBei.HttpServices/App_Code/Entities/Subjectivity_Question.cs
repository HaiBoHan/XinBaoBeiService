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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Entities
{
    /// <summary>
    /// Subjectivity_Question 的摘要说明
    /// </summary>
    public class Subjectivity_Question
    {
        public Subjectivity_Question()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private long id;

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        private string keyWords;
        //关键字
        public string KeyWords
        {
            get { return keyWords; }
            set { keyWords = value; }
        }

        private long questioner;
        //问题提出者也就是用户id
        public long Questioner
        {
            get { return questioner; }
            set { questioner = value; }
        }

        private long sub_Qes_T;
        /// <summary>
        /// 问题id，取的是专家问题的id
        /// </summary>
        public long Sub_Qes_T
        {
            get { return sub_Qes_T; }
            set { sub_Qes_T = value; }
        }
        private string sub_Qes_title;
        //问题标题：就是提的问题如您对孩子有什么认识能力的问题吗？取的是专家问题的title
        public string Sub_Qes_title
        {
            get { return sub_Qes_title; }
            set { sub_Qes_title = value; }
        }

        private string question_Content;
        //问题内容：就是用户对专家问题的回答，例如：宝宝只有三颗牙正常吗？
        public string Question_Content
        {
            get { return question_Content; }
            set { question_Content = value; }
        }

        private string aboutAge;
        //年龄：用户的年龄
        public string AboutAge
        {
            get { return aboutAge; }
            set { aboutAge = value; }
        }

        private DateTime messDate;
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime MessDate
        {
            get { return messDate; }
            set { messDate = value; }
        }


        private string subQue_Result;
        //主观问题答案：专家的回答
        public string SubQue_Result
        {
            get { return subQue_Result; }
            set { subQue_Result = value; }
        }

        public static Subjectivity_Question GetSubjectivityQuestionFromTable(DataRow row)
        {
            Subjectivity_Question question = new Subjectivity_Question();

            question.ID = UIHelper.GetLong(row["id"]);

            question.Sub_Qes_T = UIHelper.GetLong(row["sub_qes_t"]);
            question.Sub_Qes_title = UIHelper.GetString(row["sub_Qes_title"]);
            question.Question_Content = UIHelper.GetString(row["question_Content"]);
            question.KeyWords = UIHelper.GetString(row["keyWords"]);
            question.MessDate = UIHelper.GetDate(row["messDate"]);
            question.Questioner = UIHelper.GetLong(row["questioner"]);
            question.SubQue_Result = UIHelper.GetString(row["subQue_Result"]);

            question.AboutAge = UIHelper.GetString(row["aboutAge"]);
            return question;
        }

        public static List<Subjectivity_Question> GetSubjectivityQuestionFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Subjectivity_Question> list = new List<Subjectivity_Question>();
                foreach (DataRow row in table.Rows)
                {
                    Subjectivity_Question question = GetSubjectivityQuestionFromTable(row);

                    if (question != null)
                    {
                        list.Add(question);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Subjectivity_Question> GetSubjectivityQuestionFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetSubjectivityQuestionFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}