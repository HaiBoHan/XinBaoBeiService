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
    /// ParentChildCurriculum 的摘要说明
    /// </summary>
    public class ParentChildCurriculum
    {
        public ParentChildCurriculum()
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

        private string stage;

        public string Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        private string aboutAge;

        public string AboutAge
        {
            get { return aboutAge; }
            set { aboutAge = value; }
        }


        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

       
       

        public static ParentChildCurriculum GetParentChildCurriculumFromTable(DataRow row)
        {
            ParentChildCurriculum pcc = new ParentChildCurriculum();

            pcc.ID = UIHelper.GetLong(row["id"]);
            pcc.Stage = UIHelper.GetString(row["stage"]);

            pcc.AboutAge = UIHelper.GetString(row["aboutAge"]);
            pcc.Content = UIHelper.GetString(row["content"]);
            
            return pcc;
        }
        //public static int GetRowsCount(DataSet ds)
        //{
        //   List<Message> messagelst = GetMessageFromTable(ds);
        //   if (messagelst == null)
        //   {
        //       return 0;
        //   }
        //   return messagelst.Count;
        //}
        public static List<ParentChildCurriculum> GetParentChildCurriculumFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<ParentChildCurriculum> list = new List<ParentChildCurriculum>();
                foreach (DataRow row in table.Rows)
                {
                    ParentChildCurriculum pcc = GetParentChildCurriculumFromTable(row);

                    if (pcc != null)
                    {
                        list.Add(pcc);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<ParentChildCurriculum> GetParentChildCurriculumFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetParentChildCurriculumFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}