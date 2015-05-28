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
    ///UserRegister 的摘要说明
    /// </summary>
    public class UserRegister
    {
        public UserRegister()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //

        }

        // 手机号
        private string telno;

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelNo
        {
            get { return telno; }
            set { telno = value; }
        }


        // 密码
        private string pwd;

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        // 账号
        private string account;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

    }
}