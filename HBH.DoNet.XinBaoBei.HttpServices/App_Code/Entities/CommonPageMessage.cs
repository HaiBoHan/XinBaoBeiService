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
    /// CommonPageMessage 的摘要说明
    /// </summary>
    public class CommonPageMessage
    {
        public CommonPageMessage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private int pageNum;

        public int PageNum
        {
            get { return pageNum; }
            set { pageNum = value; }
        }

        private int totalPage;

        public int TotalPage
        {
            get { return totalPage; }
            set { totalPage = value; }
        }

        private int totalNum;

        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }


        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

       
    }
}