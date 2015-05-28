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
    /// Information 的摘要说明
    /// </summary>
    public class Information
    {
        public Information()
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

        private string information_type;
        //信息分类（体征，认识。。。）
        public string Information_Type
        {
            get { return information_type; }
            set { information_type = value; }
        }

        private string information_Content;
        //信息内容（例如大部分宝宝现在已经会走了。。。。）
        public string Information_Content
        {
            get { return information_Content; }
            set { information_Content = value; }
        }

        private string aboutAge;
        //年龄段
        public string AboutAge
        {
            get { return aboutAge; }
            set { aboutAge = value; }
        }

        

        public static Information GetInformationFromTable(DataRow row)
        {
            Information information = new Information();

            information.ID = UIHelper.GetLong(row["id"]);
            information.Information_Content = UIHelper.GetString(row["information_Content"]);
            information.Information_Type = UIHelper.GetString(row["information_type"]);
            information.AboutAge = UIHelper.GetString(row["aboutAge"]);

            return information;
        }

        public static List<Information> GetInformationFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Information> list = new List<Information>();
                foreach (DataRow row in table.Rows)
                {
                    Information info = GetInformationFromTable(row);

                    if (info != null)
                    {
                        list.Add(info);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Information> GetInformationFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetInformationFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}