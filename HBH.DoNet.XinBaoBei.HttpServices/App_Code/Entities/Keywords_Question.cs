using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    ///Keywords_Ask 的摘要说明
    /// </summary>
    public class Keywords_Question
    {
        public Keywords_Question()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        // 主键
        private long id;

        /// <summary>
        /// 主键
        /// </summary>
        public long ID
        {
            get { return id; }
            set { id = value; }
        }

        // 关键字
        private string keywords;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        // 关键字子项
        private List<Keywords_Question> childs;

        /// <summary>
        /// 关键字子项
        /// </summary>
        public List<Keywords_Question> Childs
        {
            get { return childs; }
            set { childs = value; }
        }
	


        public static Keywords_Question GetFromRow(DataRow row)
        {
            Keywords_Question question = new Keywords_Question();

            question.ID = UIHelper.GetLong(row["ID"]);
            question.KeyWords = UIHelper.GetString(row["KeyWords"]);
            //question.ID = UIHelper.GetLong(row["SecondID"]);
            //question.KeyWords = UIHelper.GetString(row["SecondKeyWords"]);
            //question.ID = UIHelper.GetLong(row["ThirdID"]);
            //question.KeyWords = UIHelper.GetString(row["ThirdKeyWords"]);

            return question;
        }

        public static Keywords_Question GetFromParams(long id,string keywords)
        {
            Keywords_Question question = new Keywords_Question();

            question.ID = id;
            question.KeyWords = keywords;

            return question;
        }

        public static List<Keywords_Question> GetFromTable(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Keywords_Question> list = new List<Keywords_Question>();
                //Dictionary<long, Keywords_Question> dicFirstQuestion = new Dictionary<long, Keywords_Question>();
                //Dictionary<long, Dictionary<long, Keywords_Question>> dicSecondQuestion = new Dictionary<long, Dictionary<long, Keywords_Question>>();
                //Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>> dicThirdQuestion = new Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>>();
                //Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>> dicQuestion = new Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>>();

                Dictionary<long, Keywords_Question> dicFirstQuestion = new Dictionary<long, Keywords_Question>();
                Dictionary<long, Keywords_Question> dicSecondQuestion = new Dictionary<long, Keywords_Question>();
                Dictionary<long, Keywords_Question> dicThirdQuestion = new Dictionary<long, Keywords_Question>();
                foreach (DataRow row in table.Rows)
                {
                    //Keywords_Question question = GetFromRow(row);

                    long qID = UIHelper.GetLong(row["ID"]);
                    string qKeywords = UIHelper.GetString(row["KeyWords"]);
                    long qSecondID = UIHelper.GetLong(row["SecondID"]);
                    string qSecondKeywords = UIHelper.GetString(row["SecondKeyWords"]);
                    long qThirdID = UIHelper.GetLong(row["ThirdID"]);
                    string qThirdKeywords = UIHelper.GetString(row["ThirdKeyWords"]);

                    if (!dicFirstQuestion.ContainsKey(qID))
                    {
                        // 第一级
                        Keywords_Question first = GetFromParams(qID,qKeywords);
                        first.Childs = new List<Keywords_Question>();

                        dicFirstQuestion.Add(qID, first);
                        list.Add(first);

                        // 第二级
                        Keywords_Question second = GetFromParams(qSecondID, qSecondKeywords);
                        second.Childs = new List<Keywords_Question>();

                        first.Childs.Add(second);
                        dicSecondQuestion.Add(qSecondID, second);

                        // 第三级
                        Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                        second.Childs.Add(third);
                        dicThirdQuestion.Add(qThirdID, third);
                    }
                    else if(!dicSecondQuestion.ContainsKey(qSecondID))
                    {
                        // 第一级,取字典
                        Keywords_Question first = dicFirstQuestion[qID];
                        if (first.Childs == null)
                        {
                            first.Childs = new List<Keywords_Question>();
                        }

                        // 第二级
                        Keywords_Question second = GetFromParams(qSecondID, qSecondKeywords);
                        second.Childs = new List<Keywords_Question>();

                        first.Childs.Add(second);
                        dicSecondQuestion.Add(qSecondID, second);

                        // 第三级
                        Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                        second.Childs.Add(third);
                        dicThirdQuestion.Add(qThirdID, third);
                    }
                    else if (!dicThirdQuestion.ContainsKey(qSecondID))
                    {
                        //// 第一级,取字典
                        //Keywords_Question first = dicFirstQuestion[qID];
                        //if (first.Childs == null)
                        //{
                        //    first.Childs = new List<Keywords_Question>();
                        //}

                        // 第二级,取字典
                        Keywords_Question second = dicSecondQuestion[qSecondID];
                        if (second.Childs == null)
                        {
                            second.Childs = new List<Keywords_Question>();
                        }

                        // 第三级
                        Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                        second.Childs.Add(third);
                        dicThirdQuestion.Add(qThirdID, third);
                    }
                }

                return list;
            }

            return null;
        }

        public static List<Keywords_Question> GetFromDataSet(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetFromTable(ds.Tables[0]);
            }

            return null;

            //return EntityHelper.GetFromDataSet<Keywords_Question>(ds, GetFromRow);
        }



        public static List<Keywords_Question> GetFromTable2(DataTable table)
        {
            if (table.Rows != null
                && table.Rows.Count > 0
                )
            {
                List<Keywords_Question> list = new List<Keywords_Question>();
                //Dictionary<long, Keywords_Question> dicFirstQuestion = new Dictionary<long, Keywords_Question>();
                //Dictionary<long, Dictionary<long, Keywords_Question>> dicSecondQuestion = new Dictionary<long, Dictionary<long, Keywords_Question>>();
                //Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>> dicThirdQuestion = new Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>>();
                //Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>> dicQuestion = new Dictionary<long, Dictionary<long, Dictionary<long, Keywords_Question>>>();

                Dictionary<long, Keywords_Question> dicFirstQuestion = new Dictionary<long, Keywords_Question>();
                Dictionary<long, Keywords_Question> dicSecondQuestion = new Dictionary<long, Keywords_Question>();
                //Dictionary<long, Keywords_Question> dicThirdQuestion = new Dictionary<long, Keywords_Question>();
                foreach (DataRow row in table.Rows)
                {
                    //Keywords_Question question = GetFromRow(row);

                    //long qID = UIHelper.GetLong(row["ID"]);
                    //string qKeywords = UIHelper.GetString(row["KeyWords"]);
                    //long qSecondID = UIHelper.GetLong(row["SecondID"]);
                    //string qSecondKeywords = UIHelper.GetString(row["SecondKeyWords"]);
                    //long qThirdID = UIHelper.GetLong(row["ThirdID"]);
                    //string qThirdKeywords = UIHelper.GetString(row["ThirdKeyWords"]);

                    long qID = UIHelper.GetLong(row["SecondID"]);
                    string qKeywords = UIHelper.GetString(row["SecondKeyWords"]);
                    long qSecondID = UIHelper.GetLong(row["ThirdID"]);
                    string qSecondKeywords = UIHelper.GetString(row["ThirdKeyWords"]);

                    if (!dicFirstQuestion.ContainsKey(qID))
                    {
                        // 第一级
                        Keywords_Question first = GetFromParams(qID, qKeywords);
                        first.Childs = new List<Keywords_Question>();

                        dicFirstQuestion.Add(qID, first);
                        list.Add(first);

                        // 第二级
                        Keywords_Question second = GetFromParams(qSecondID, qSecondKeywords);
                        second.Childs = new List<Keywords_Question>();

                        first.Childs.Add(second);
                        dicSecondQuestion.Add(qSecondID, second);

                        //// 第三级
                        //Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                        //second.Childs.Add(third);
                        //dicThirdQuestion.Add(qThirdID, third);
                    }
                    else if (!dicSecondQuestion.ContainsKey(qSecondID))
                    {
                        // 第一级,取字典
                        Keywords_Question first = dicFirstQuestion[qID];
                        if (first.Childs == null)
                        {
                            first.Childs = new List<Keywords_Question>();
                        }

                        // 第二级
                        Keywords_Question second = GetFromParams(qSecondID, qSecondKeywords);
                        second.Childs = new List<Keywords_Question>();

                        first.Childs.Add(second);
                        dicSecondQuestion.Add(qSecondID, second);

                        //// 第三级
                        //Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                        //second.Childs.Add(third);
                        //dicThirdQuestion.Add(qThirdID, third);
                    }
                    //else if (!dicThirdQuestion.ContainsKey(qSecondID))
                    //{
                    //    //// 第一级,取字典
                    //    //Keywords_Question first = dicFirstQuestion[qID];
                    //    //if (first.Childs == null)
                    //    //{
                    //    //    first.Childs = new List<Keywords_Question>();
                    //    //}

                    //    // 第二级,取字典
                    //    Keywords_Question second = dicSecondQuestion[qSecondID];
                    //    if (second.Childs == null)
                    //    {
                    //        second.Childs = new List<Keywords_Question>();
                    //    }

                    //    // 第三级
                    //    Keywords_Question third = GetFromParams(qThirdID, qThirdKeywords);
                    //    second.Childs.Add(third);
                    //    dicThirdQuestion.Add(qThirdID, third);
                    //}
                }

                return list;
            }

            return null;
        }

        public static List<Keywords_Question> GetFromDataSet2(DataSet ds)
        {
            if (ds != null
                && ds.Tables != null
                && ds.Tables.Count > 0
                )
            {
                return GetFromTable2(ds.Tables[0]);
            }

            return null;

            //return EntityHelper.GetFromDataSet<Keywords_Question>(ds, GetFromRow);
        }
    }
}