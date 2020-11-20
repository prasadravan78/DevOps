using System;
using System.Collections.Generic;
using System.Text;

namespace Extract.DevOps
{
    public class Constants
    {
        #region Common

        public class Common
        {
            public const string PERSONAL_ACCESS_TOKEN = "personalAccessToken";
            public const string GET_BACKLOG_LEVEL_WORK_ITEMS_URL = "getBacklogLevelWorkItemsUrl";
            public const string GET_WORK_ITEM_BY_ID_URL = "getWorkItemByIdUrl";
        }

        #endregion Common

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
