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
    /// Judge_Result 的摘要说明
    /// </summary>
    public class Judge_Result
    {
        public Judge_Result()
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

        private long userID;
        //回答者编码
        public long UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string measureID;
        //测评题ID
        public string MeasureID
        {
            get { return measureID; }
            set { measureID = value; }
        }

        private string result;
        //答案
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

      

        public static Judge_Result GetJudge_ResultFromTable(DataRow row)
        {
            Judge_Result result = new Judge_Result();

            result.ID = UIHelper.GetLong(row["id"]);
            result.MeasureID = UIHelper.GetString(row["measureID"]);
            result.Result = UIHelper.GetString(row["result"]);

            result.UserID = UIHelper.GetLong(row["userID"]);
            return result;
        }

        public static List<Judge_Result> GetJudge_ResultFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Judge_Result> list = new List<Judge_Result>();
                foreach (DataRow row in table.Rows)
                {
                    Judge_Result judge_Result = GetJudge_ResultFromTable(row);

                    if (judge_Result != null)
                    {
                        list.Add(judge_Result);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Judge_Result> GetJudge_ResultFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetJudge_ResultFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}