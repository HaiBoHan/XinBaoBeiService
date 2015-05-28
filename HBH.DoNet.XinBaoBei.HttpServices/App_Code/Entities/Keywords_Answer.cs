using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Entities
{
    /// <summary>
    ///Keywords_Answer 的摘要说明
    /// </summary>
    public class Keywords_Answer
    {
        public Keywords_Answer()
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




        // 关键字主键
        private long id;

        /// <summary>
        /// 关键字主键
        /// </summary>
        public long ID
        {
            get { return id; }
            set { id = value; }
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


    }
}