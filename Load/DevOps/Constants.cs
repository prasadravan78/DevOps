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

        #endregion Common

        #region Work Item Type

        public class WorkItemType
        {
            public const string PRODUCT_BACKLOG_ITEM = "Product Backlog Item";
        }

        #endregion Work Item Type

        #region URL Parameters

        public class URLParameters
        {
            public const string ORGANISATION_NAME = "{Organisation}";
            public const string PROJECT_NAME = "{Project}";
            public const string PROJECT_TEAM_NAME = "{ProjectTeam}";
        }

        #endregion URL Parameters
    }
}
