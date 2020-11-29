namespace Load.DevOpsToDevOps.DevOpsWorkItem
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Load.DevOps;
    using Load.DevOpsToDevOps.Models;
    using System.Configuration;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
    using Microsoft.VisualStudio.Services.Common;
    using Microsoft.VisualStudio.Services.WebApi;
    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
    using Newtonsoft.Json;

    public class DevOpsWorkItem : IDevOpsWorkItem
    {
        #region Members

        List<string> workItemFields = new List<string> 
        { 
            "System.Title", "Microsoft.VSTS.Common.AcceptanceCriteria", "System.Description", "Microsoft.VSTS.Common.ValueArea", 
            "Microsoft.VSTS.Scheduling.Effort", "Microsoft.VSTS.Common.BusinessValue", "System.CreatedDate", "System.ChangedDate", 
            "Microsoft.VSTS.Common.StateChangeDate", "System.Id" 
        };

        #endregion Members

        #region Public Methods

        public void CreateWorkItem(DevOpsParameters devOpsParameters, WorkItem workItem)
        {
            var restApiDetails = GetDetailsForRestApiCall(Constants.Common.CREATE_WORK_ITEM_URL, devOpsParameters);

            try
            {                  
                string type = Constants.WorkItemType.PRODUCT_BACKLOG_ITEM;
                JsonPatchDocument workItemCreateModels = new JsonPatchDocument();
                string organization = devOpsParameters.OrganisationName;
                string project = devOpsParameters.ProjectName;
                string _UrlServiceCreate = restApiDetails.Url;
                VssConnection connection = new VssConnection(new Uri(_UrlServiceCreate), new VssBasicCredential(string.Empty, restApiDetails.PAT));
                WorkItemTrackingHttpClient workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();

                foreach (var prop in workItem.Fields)
                {
                    if (workItemFields.Contains(prop.Key))
                    {
                        workItemCreateModels.Add(new JsonPatchOperation
                                                 {
                                                     Operation = Operation.Add,
                                                     Path = Constants.Common.WORK_ITEM_FIELD_PATH + prop.Key,
                                                     Value = prop.Value.ToString()
                                                 });
                    }
                }

                //foreach (var relation in workItem.Relations)
                //{
                //    workItemCreateModels.Add(
                //           new JsonPatchOperation
                //           {
                //               Operation = Operation.Add,
                //               Path = Constants.Common.WORK_ITEM_RELATION_PATH,
                //               Value = new
                //               {
                //                   rel = relation.Rel,
                //                   url = relation.Url
                //               }
                //           });
                //}

                var WorkItemValue = new StringContent(JsonConvert.SerializeObject(workItemCreateModels), Encoding.UTF8, "application/json-patch+json");
                WorkItem clonedWi = workItemTrackingHttpClient.CreateWorkItemAsync(workItemCreateModels, project, type).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion Public Methods

        #region Private Methods

        private RestApiDetails GetDetailsForRestApiCall(string urlRequired, DevOpsParameters devOpsParameters)
        {
            RestApiDetails restApiDetails = new RestApiDetails
            {
                PAT = ConfigurationManager.AppSettings[Constants.Common.PERSONAL_ACCESS_TOKEN],
                Url = ConfigurationManager.AppSettings[urlRequired]
            };

            if (!string.IsNullOrEmpty(restApiDetails.Url))
            {
                restApiDetails.Url = restApiDetails.Url.Replace(Constants.URLParameters.ORGANISATION_NAME, devOpsParameters.OrganisationName)
                                                       .Replace(Constants.URLParameters.PROJECT_NAME, devOpsParameters.ProjectName)
                                                       .Replace(Constants.URLParameters.PROJECT_TEAM_NAME, devOpsParameters.ProjectTeamName);
            }

            return restApiDetails;
        }

        #endregion Private Methods
    }
}
