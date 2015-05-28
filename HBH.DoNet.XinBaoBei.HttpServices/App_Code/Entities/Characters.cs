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
    /// Characters 的摘要说明
    /// </summary>
    public class Characters
    {
        public Characters()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }



        public static Characters GetCharactersFromTable(DataRow row)
        {
            Characters entity = new Characters();

            entity.Name = UIHelper.GetString(row["Variable_name"]);
            entity.Value = UIHelper.GetString(row["Value"]);

            return entity;
        }

        public static List<Characters> GetCharactersFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Characters> list = new List<Characters>();
                foreach (DataRow row in table.Rows)
                {
                    Characters entity = GetCharactersFromTable(row);

                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Characters> GetCharactersFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetCharactersFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}