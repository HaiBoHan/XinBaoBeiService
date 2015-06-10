using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    ///Keywords_Answer 的摘要说明
    /// </summary>
    public class Keywords_QuestionHistory
    {
        public Keywords_QuestionHistory()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        // 用户ID
        private long userID;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        // 用户账号
        private string userAccount;

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount
        {
            get { return userAccount; }
            set { userAccount = value; }
        }




        // 问题关键字主键
        private long questionID;

        /// <summary>
        /// 问题关键字主键
        /// </summary>
        public long QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }

        // 关键字
        private string keywords;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        // 提问时间
        private string createdTime;

        /// <summary>
        /// 提问时间
        /// </summary>
        public string CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        // 年龄
        private string age;

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        // 答案
        private string solution;

        /// <summary>
        /// 答案
        /// </summary>
        public string Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        // 答案简介
        private string solutionSummary;

        /// <summary>
        /// 答案简介
        /// </summary>
        public string SolutionSummary
        {
            get { return solutionSummary; }
            set { solutionSummary = value; }
        }


        public static Keywords_QuestionHistory GetFromRow(DataRow row)
        {
            Keywords_QuestionHistory question = new Keywords_QuestionHistory();

            question.UserID = UIHelper.GetLong(row["UserID"]);
            question.UserAccount = UIHelper.GetString(row["UserAccount"]);
            question.KeyWords = UIHelper.GetString(row["KeyWords"]);
            question.CreatedTime = UIHelper.GetString(row["CreatedTime"]);
            question.Age = UIHelper.GetString(row["Age"]);
            question.QuestionID = UIHelper.GetLong(row["QuestionID"]);
            question.Solution = UIHelper.GetString(row["Solution"]);
            question.SolutionSummary = UIHelper.GetString(row["SolutionSummary"]);

            return question;
        }

        public static List<Keywords_QuestionHistory> GetFromDataSet(DataSet ds)
        {
            //if (ds != null
            //    && ds.Tables != null
            //    && ds.Tables.Count > 0
            //    )
            //{
            //    return GetFromTable(ds.Tables[0]);
            //}

            //return null;

            return EntityHelper.GetFromDataSet<Keywords_QuestionHistory>(ds, GetFromRow);
        }
    }
}