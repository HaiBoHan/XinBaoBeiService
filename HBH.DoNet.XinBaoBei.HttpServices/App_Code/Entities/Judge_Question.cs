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
    /// Judge_Question 的摘要说明
    /// </summary>
    public class Judge_Question
    {
        public Judge_Question()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private string question_Title;

        public string Question_Title
        {
            get { return question_Title; }
            set { question_Title = value; }
        }

        private List<Judge_Question_Option> question_Option;

        public List<Judge_Question_Option> Question_Option
        {
            get { return question_Option; }
            set { question_Option = value; }
        }


        public static Judge_Question GetJudgeQuestionFromTable(DataRow row)
        {
            Judge_Question question = new Judge_Question();

            question.Question_Title = UIHelper.GetString(row["question_Title"]);
            //这个List需要测试一下修改一下的。
            // question.Question_Option = UIHelper.GetString(row["question_Option"]);

            return question;
        }

        public static List<Judge_Question> GetJudgeQuestionFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Judge_Question> list = new List<Judge_Question>();
                foreach (DataRow row in table.Rows)
                {
                    Judge_Question question = GetJudgeQuestionFromTable(row);

                    if (question != null)
                    {
                        list.Add(question);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Judge_Question> GetJudgeQuestionFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetJudgeQuestionFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}