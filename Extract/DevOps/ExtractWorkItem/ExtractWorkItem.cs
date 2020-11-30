namespace Extract.DevOps.ExtractWorkItem
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Extract.DevOps.Models;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ExtractWorkItem : IExtractWorkItem
    {
        #region Public Methods

        public async Task<List<string>> GetProjectsByOrganisation(DevOpsParameters devOpsParameters)
        {
            List<string> projectNames = new List<string>();
            var restApiDetails = GetDetailsForRestApiCall(Constants.Common.GET_PROJECTS_URL, devOpsParameters);

            if (restApiDetails != null)
            {
                try
                {
                    var organization = devOpsParameters.OrganisationName;
                    var project = devOpsParameters.ProjectName;
                    var ProjectTeam = devOpsParameters.ProjectTeamName;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                                                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", 
                                                                                                                                                                 "", 
                                                                                                                                                                 restApiDetails.PAT))));

                        using (HttpResponseMessage response = client.GetAsync(
                                    restApiDetails.Url).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            JObject jsonObject = JObject.Parse(responseBody);
                            projectNames = jsonObject["value"].Select(k => k["name"].ToString()).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return projectNames;
        }

        public async Task<BacklogLevelWorkItems> GetBacklogLevelWorkItemsAsync(DevOpsParameters devOpsParameters)
        {
            BacklogLevelWorkItems backlogLevelWorkItems = new BacklogLevelWorkItems();
            var restApiDetails = GetDetailsForRestApiCall(Constants.Common.GET_BACKLOG_LEVEL_WORK_ITEMS_URL, devOpsParameters);

            if (restApiDetails != null)
            {
                try
                {
                    var organization = devOpsParameters.OrganisationName;
                    var project = devOpsParameters.ProjectName;
                    var ProjectTeam = devOpsParameters.ProjectTeamName;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(
                                Encoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", "", restApiDetails.PAT))));

                        using (HttpResponseMessage response = client.GetAsync(
                                    restApiDetails.Url).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();

                            backlogLevelWorkItems = JsonConvert.DeserializeObject<BacklogLevelWorkItems>(responseBody);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return backlogLevelWorkItems;
        }

        public async Task<List<WorkItem>> GetWorkItemsAsync(DevOpsParameters devOpsParameters, BacklogLevelWorkItems backlogLevelWorkItems)
        {
            List<WorkItem> workItems = new List<WorkItem>();

            try
            {
                var restApiDetails = GetDetailsForRestApiCall(Constants.Common.GET_WORK_ITEM_BY_ID_URL, devOpsParameters);

                if (restApiDetails != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(
                                Encoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", "", restApiDetails.PAT))));

                        if (backlogLevelWorkItems != null)
                        {
                            foreach (var workItemLink in backlogLevelWorkItems.WorkItems)
                            {
                                using (HttpResponseMessage response = client.GetAsync(
                                    string.Format(restApiDetails.Url, workItemLink.Target.Id)).Result)
                                {
                                    response.EnsureSuccessStatusCode();
                                    string responseBody = await response.Content.ReadAsStringAsync();

                                    workItems.Add(JsonConvert.DeserializeObject<WorkItem>(responseBody));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return workItems;
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
