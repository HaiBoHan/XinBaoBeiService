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
    /// Measure 的摘要说明
    /// </summary>
    public class Measure
    {
        public Measure()
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

        private string measure_Title;
        
        public string Measure_Title
        {
            get { return measure_Title; }
            set { measure_Title = value; }
        }


        private string measure_Type;

        public string Measure_Type
        {
            get { return measure_Type; }
            set { measure_Type = value; }
        }

        private string measure_xml_path;

        public string Measure_Xml_Path
        {
            get { return measure_xml_path; }
            set { measure_xml_path = value; }
        }

        private string measure_result_xml_path;

        public string Measure_Result_Xml_Path
        {
            get { return measure_result_xml_path; }
            set { measure_result_xml_path = value; }
        }


        public static Measure GetMeasureFromTable(DataRow row)
        {
            Measure measure = new Measure();

            measure.ID = UIHelper.GetLong(row["id"]);
            measure.Measure_Type = UIHelper.GetString(row["measure_type"]);
            measure.Measure_Title = UIHelper.GetString(row["measure_title"]);
            measure.Measure_Xml_Path = UIHelper.GetString(row["measure_xml_path"]);
            measure.Measure_Result_Xml_Path = UIHelper.GetString(row["measure_result_xml_path"]);

            return measure;
        }

        public static List<Measure> GetMeasureFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Measure> list = new List<Measure>();
                foreach (DataRow row in table.Rows)
                {
                    Measure measure = GetMeasureFromTable(row);

                    if (measure != null)
                    {
                        list.Add(measure);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Measure> GetMeasureFromTable(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetMeasureFromTable(ds.Tables[0]);
            }

            return null;
        }
    }
}