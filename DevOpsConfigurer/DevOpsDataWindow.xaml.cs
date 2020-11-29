namespace DevOpsConfigurer
{    
    using System.Collections.Generic;
    using System.Windows;
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

        public DevOpsDataWindow(List<WorkItem> workItems)
        {
            IDevOpsWorkItem devOpsWorkItem = new DevOpsWorkItem();
            DevOpsParameters devOpsParameters = new DevOpsParameters
            {
                OrganisationName = "OrganizationRP2",
                ProjectName = "ProjectRP2",
                ProjectTeamName = "ProjectRP2 Team"
            };

            if (workItems != null)
            {
                foreach (var workItem in workItems)
                {
                    devOpsWorkItem.CreateWorkItem(devOpsParameters, workItem);
                }
            }            
        }
    }
}
