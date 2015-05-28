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
    /// Judge_Question_Option 的摘要说明
    /// </summary>
    public class Judge_Question_Option
    {
        public Judge_Question_Option()
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

        private string option;

        public string Option
        {
            get { return option; }
            set { option = value; }
        }


        public static Judge_Question_Option GetQuestionOptionFromTable(DataRow row)
        {
            Judge_Question_Option question_option = new Judge_Question_Option();

            question_option.ID = UIHelper.GetLong(row["id"]);
            question_option.Option = UIHelper.GetString(row["option"]);
            return question_option;
        }

        public static List<Judge_Question_Option> GetQuestionOptionFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Judge_Question_Option> list = new List<Judge_Question_Option>();
                foreach (DataRow row in table.Rows)
                {
                    Judge_Question_Option question_option = GetQuestionOptionFromTable(row);

                    if (question_option != null)
                    {
                        list.Add(question_option);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Judge_Question_Option> GetQuestionOptionFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetQuestionOptionFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}