using DevOpsConfigurer.Models;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Process.WebApi.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DevOpsConfigurer
{
    /// <summary>
    /// Interaction logic for DevOpsDataWindow.xaml
    /// </summary>
    public partial class DevOpsDataWindow : Window
    {
        public DevOpsDataWindow()
        {
            InitializeComponent();
        }

        public DevOpsDataWindow(List<WorkItem> workItems)
        {
            var grid = CreateGrid();
            foreach (var workItem in workItems)
            {
                //CreateDataRow(dataGrid, workItem);
                CreateWorkItem(workItem);
            }

            Content = dataGrid;
        }
        private Grid CreateGrid()
        {
            Grid grid = new Grid();

            grid.Width = 400;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.ShowGridLines = true;

            ColumnDefinition chkBoxColumn = new ColumnDefinition();
            ColumnDefinition workItemNameColumn = new ColumnDefinition();

            grid.ColumnDefinitions.Add(chkBoxColumn);
            grid.ColumnDefinitions.Add(workItemNameColumn);

            return grid;
        }

        private void CreateDataRow(Grid dataGrid, WorkItem workItem)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(45);
            dataGrid.RowDefinitions.Add(rowDefinition);

            CheckBox checkBox = new CheckBox();
            Label label = new Label { Content = workItem.Fields.Where(k => k.Key.Contains("Title")).Select(k => k.Value)?.FirstOrDefault() };

            Grid.SetRow(checkBox, dataGrid.RowDefinitions.Count());
            Grid.SetColumn(checkBox, 0);
            Grid.SetRow(label, dataGrid.RowDefinitions.Count());
            Grid.SetColumn(label, 1);

            dataGrid.Children.Add(checkBox);
            dataGrid.Children.Add(label);
        }

        private void CreateWorkItem(WorkItem workItem)
        {
            try
            {
                var workItemFields = new List<string> { "System.Title", "Microsoft.VSTS.Common.AcceptanceCriteria", "System.Description", "Microsoft.VSTS.Common.ValueArea", "Microsoft.VSTS.Scheduling.Effort", "Microsoft.VSTS.Common.BusinessValue", "System.CreatedDate", "System.ChangedDate", "Microsoft.VSTS.Common.StateChangeDate", "System.Id" };
                var personalAccessToken = "rvvn2rcdofmbs7jle4nnaxrt7jpfyz7pr2pa5rrsftxtneqxwqtq";
                string type = "Product Backlog Item";
                JsonPatchDocument workItemCreateModels = new JsonPatchDocument();
 

                string organization = "OrganizationRP2";
                string project = "ProjectRP2";
                //string _UrlServiceCreate = $"https://dev.azure.com/{organization}/{project}/_apis/wit/workitems/${type}?api-version=6.1-preview.3";

                string _UrlServiceCreate = $"https://dev.azure.com/{organization}";
                VssConnection connection = new VssConnection(new Uri(_UrlServiceCreate), new VssBasicCredential(string.Empty, personalAccessToken));

                WorkItemTrackingHttpClient workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();


                //WorkItem wiToClone = workItemTrackingHttpClient.GetWorkItemAsync(Convert.ToInt32(workItem.Id), expand: WorkItemExpand.Relations).Result;

                foreach (var prop in workItem.Fields)
                {   
                    if (workItemFields.Contains(prop.Key))
                    {
                        workItemCreateModels.Add(
                            new JsonPatchOperation
                            {
                                Operation = Operation.Add,
                                Path = "/fields/" + prop.Key,
                                Value = prop.Value.ToString()
                            });
                    }
                }

                foreach (var relation in workItem.Relations)
                {
                    workItemCreateModels.Add(
                           new JsonPatchOperation
                           {
                               Operation = Operation.Add,
                               Path = "/relations/-" ,
                               Value = new
                               {
                                   rel = relation.Rel,
                                   url = relation.Url
                               }
                           });
                }

                var WorkItemValue = new StringContent(JsonConvert.SerializeObject(workItemCreateModels), Encoding.UTF8, "application/json-patch+json");
                var response = string.Empty;

                WorkItem clonedWi = workItemTrackingHttpClient.CreateWorkItemAsync(workItemCreateModels, project, type).Result;

                //using (HttpClient client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Accept.Add(
                //        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                //        Convert.ToBase64String(
                //            System.Text.ASCIIEncoding.ASCII.GetBytes(
                //                string.Format("{0}:{1}", "", personalAccessToken))));

                //    using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), _UrlServiceCreate) { Content = WorkItemValue })
                //    {
                //        var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;
                //        if (httpResponseMessage.IsSuccessStatusCode)
                //            response = httpResponseMessage.Content.ReadAsStringAsync().Result;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
