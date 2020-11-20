using System;
using System.Collections.Generic;
using System.Text;

namespace Load.DevOps
{
    public class Constants
    {
        #region Common

        public class Common
        {
            public const string PERSONAL_ACCESS_TOKEN = "personalAccessToken";
            public const string CREATE_WORK_ITEM_URL = "createWorkItem";
            public const string WORK_ITEM_FIELD_PATH = "/fields/";
            public const string WORK_ITEM_RELATION_PATH = "/relations/";
        }

        public class WorkItemType
        {
            public const string PRODUCT_BACKLOG_ITEM = "Product Backlog Item";
        }

        #endregion Common
    }
}
