namespace DevOpsConfigurer
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using DevOpsConfigurer.Models;
    using Load.DevOpsToDevOps.DevOpsWorkItem;
    using Load.DevOpsToDevOps.Models;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    /// <summary>
    /// Interaction logic for DevOpsDataWindow.xaml
    /// </summary>
    public partial class DevOpsDataWindow : Window
    {
        public DevOpsDataWindow()
        {
            InitializeComponent();
        }

        public static List<WorkItem> extractedWorkItems = null;
        public DevOpsDataWindow(List<WorkItem> workItems)
        {
            InitializeComponent();
            List<WorkItemViewModel> workItemViewModels = new List<WorkItemViewModel>();            

            if (workItems != null)
            {
                extractedWorkItems = workItems;
                foreach (var workItem in workItems)
                {
                    object title = "";
                    workItem.Fields.TryGetValue("System.Title", out title);
                    workItemViewModels.Add(new WorkItemViewModel { Id = Convert.ToInt32(workItem.Id) , Title = title.ToString() });
                    //devOpsWorkItem.CreateWorkItem(devOpsParameters, workItem);
                }

                workItemsDataGrid.ItemsSource = workItemViewModels;
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (extractedWorkItems != null)
            {
                IDevOpsWorkItem devOpsWorkItem = new DevOpsWorkItem();
                DevOpsParameters devOpsParameters = new DevOpsParameters
                {
                    OrganisationName = "OrganizationRP2",
                    ProjectName = "ProjectRP2",
                    ProjectTeamName = "ProjectRP2 Team"
                };

                foreach (var workItem in extractedWorkItems)
                {
                    devOpsWorkItem.CreateWorkItem(devOpsParameters, workItem);
                }
            }
        }
    }
}
