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
    /// AboutUs 的摘要说明
    /// </summary>
    public class AboutUs
    {
        public AboutUs()
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



        private string aboutUs_Content;

        public string AboutUs_Content
        {
            get { return aboutUs_Content; }
            set { aboutUs_Content = value; }
        }


        public static AboutUs GetAboutUsFromTable(DataRow row)
        {
            AboutUs aboutUs = new AboutUs();
            aboutUs.AboutUs_Content = UIHelper.GetString(row["aboutUs_Content"]);

            return aboutUs;
        }

        public static List<AboutUs> GetAboutUsFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<AboutUs> list = new List<AboutUs>();
                foreach (DataRow row in table.Rows)
                {
                    AboutUs aboutUs = GetAboutUsFromTable(row);

                    if (aboutUs != null)
                    {
                        list.Add(aboutUs);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<AboutUs> GetAboutUsFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetAboutUsFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}