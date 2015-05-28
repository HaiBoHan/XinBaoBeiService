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
    /// JudgeBranch 的摘要说明
    /// </summary>
    public class JudgeBranch
    {
        public JudgeBranch()
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

       

        public static JudgeBranch GetJudgeBranchFromTable(DataRow row)
        {
            JudgeBranch message = new JudgeBranch();

            message.ID = UIHelper.GetLong(row["id"]);
            message.Name = UIHelper.GetString(row["branchname"]);

            return message;
        }

        public static List<JudgeBranch> GetJudgeBranchFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<JudgeBranch> list = new List<JudgeBranch>();
                foreach (DataRow row in table.Rows)
                {
                    JudgeBranch message = GetJudgeBranchFromTable(row);

                    if (message != null)
                    {
                        list.Add(message);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<JudgeBranch> GetJudgeBranchFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetJudgeBranchFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}