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
    /// User 的摘要说明
    /// </summary>
    public class User
    {
        public User()
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

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string region;

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string passwd;

        public string Passwd
        {
            get { return passwd; }
            set { passwd = value; }
        }

        private string sex;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        private string age;

        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        private string birthday;

        public string Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        private string pic;

        public string Pic
        {
            get { return pic; }
            set { pic = value; }
        }


        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string tel;

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        private string judge_user_account;

        public string Judge_user_account
        {
            get { return judge_user_account; }
            set { judge_user_account = value; }
        }

        //private string modifiedBy;

        //public string ModifiedBy
        //{
        //    get { return modifiedBy; }
        //    set { modifiedBy = value; }
        //} 
        //private string createdBy;

        //public string CreatedBy
        //{
        //    get { return createdBy; }
        //    set { createdBy = value; }
        //}
        //private DateTime createdOn;

        //public DateTime CreatedOn
        //{
        //    get { return createdOn; }
        //    set { createdOn = value; }
        //}
        //private DateTime modifiedOn;

        //public DateTime ModifiedOn
        //{
        //    get { return modifiedOn; }
        //    set { modifiedOn = value; }
        //}
        private long sysVersion;

        public long SysVersion
        {
            get { return sysVersion; }
            set { sysVersion = value; }
        }
        private long branch_id;

        public long Branch_ID
        {
            get { return branch_id; }
            set { branch_id = value; }
        }
        private string branch_name;

        public string Branch_Name
        {
            get { return branch_name; }
            set { branch_name = value; }
        }
        private long unit_id;

        public long Unit_id
        {
            get { return unit_id; }
            set { unit_id = value; }
        }

        private string hintMessage;
        public string HintMessage
        {
            get
            {
                return hintMessage;
            }
            set
            {
                hintMessage = value;
            }
        }

        // 签名
        private string selfSign;

        /// <summary>
        /// 签名
        /// </summary>
        public string SelfSign
        {
            get { return selfSign; }
            set { selfSign = value; }
        }

        // 宝宝姓名
        private string babyName;

        /// <summary>
        /// 宝宝姓名
        /// </summary>
        public string BabyName
        {
            get { return babyName; }
            set { babyName = value; }
        }


        // 省
        private string province;

        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        // 市
        private string city;

        /// <summary>
        /// 市
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        // 县/区
        private string county;

        /// <summary>
        /// 县/区
        /// </summary>
        public string County
        {
            get { return county; }
            set { county = value; }
        }

	
        
        public static List<User> GetUserFromPost(string strQueryResult)
        {

            DataTable userTable = Newtonsoft.Json.JsonConvert.DeserializeObject(strQueryResult) as DataTable;
            List<User> lstuser = new List<User>();
           // lstuser.Add(user);
            if (lstuser != null && lstuser.Count > 0)
            {
                
                return lstuser;
            }
            return null;
        }
       
        public static User GetUserFromTable(DataRow row)
        {
            User user = new User();

            user.Address = UIHelper.GetString(row["address"]);
            user.Region = UIHelper.GetString(row["region"]);
            user.Account = UIHelper.GetString(row["account"]);
            user.Passwd = UIHelper.GetString(row["passwd"]);
            user.Name = UIHelper.GetString(row["name"]);
            user.Sex = UIHelper.GetString(row["sex"]);

            object objBirth = row["birthday"];
            if (objBirth != null)
            {
                DateTime dt;
                if (DateTime.TryParse(objBirth.ToString(), out dt))
                {
                    user.Birthday = dt.ToString("yyyy-MM-dd");
                }
            }

            user.ID = UIHelper.GetLong(row["id"]);
            user.Pic = UIHelper.GetString(row["pic"]);
            user.Tel = UIHelper.GetString(row["tel"]);
            user.Judge_user_account = UIHelper.GetString(row["judge_user_account"]);
            user.SysVersion = UIHelper.GetLong(row["sysversion"]);
            //user.CreatedBy = UIHelper.GetString(row["createdby"]);
            //user.CreatedOn = PubClass.GetDateTime(row["createdon"],new DateTime(1900,1,1));
            //user.ModifiedBy = UIHelper.GetString(row["modifiedby"]);
            //user.ModifiedOn = PubClass.GetDateTime(row["modifiedon"], new DateTime(1900, 1, 1));

            user.Age = UIHelper.GetString(row["age"]);
            user.Branch_ID = UIHelper.GetLong(row["branch_id"]);
            user.Unit_id = UIHelper.GetLong(row["unit_id"]);
            user.Branch_Name = UIHelper.GetString(row["branch_name"]);
            user.HintMessage = UIHelper.GetString(row["hintmessage"]);
            user.SelfSign = UIHelper.GetString(row["selfsign"]);
            user.BabyName = UIHelper.GetString(row["BabyName"]);

            // 省市县区
            user.Province = UIHelper.GetString(row["Province"]);
            user.City = UIHelper.GetString(row["City"]);
            user.County = UIHelper.GetString(row["County"]);

            return user;
        }

        public static List<User> GetUserFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<User> list = new List<User>();
                foreach (DataRow row in table.Rows)
                {
                    User user = GetUserFromTable(row);

                    if (user != null)
                    {
                        list.Add(user);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<User> GetUserFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetUserFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}