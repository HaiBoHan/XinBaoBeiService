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
    /// JudgeUnit 的摘要说明
    /// </summary>
    public class JudgeUnit
    {
        public JudgeUnit()
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

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

       

        public static JudgeUnit GetJudgeUnitFromTable(DataRow row)
        {
            JudgeUnit message = new JudgeUnit();

            message.ID = UIHelper.GetLong(row["id"]);
            message.Name = UIHelper.GetString(row["Unitname"]);

            return message;
        }

        public static List<JudgeUnit> GetJudgeUnitFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<JudgeUnit> list = new List<JudgeUnit>();
                foreach (DataRow row in table.Rows)
                {
                    JudgeUnit message = GetJudgeUnitFromTable(row);

                    if (message != null)
                    {
                        list.Add(message);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<JudgeUnit> GetJudgeUnitFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetJudgeUnitFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}