using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Entities
{
    /// <summary>
    /// SubmitSubQuestion 的摘要说明
    /// </summary>
    public class SubmitSubQuestion
    {
        public SubmitSubQuestion()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }
        private long userid;

        public long UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        private long questionid;

        public long QuestionID
        {
            get { return questionid; }
            set { questionid = value; }
        }

        private string age;

        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        private string questiontitle;

        public string QuestionTitle
        {
            get { return questiontitle; }
            set { questiontitle = value; }
        }
        private string[] keyword;

        public string[] Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

    }
}