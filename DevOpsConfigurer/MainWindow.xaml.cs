namespace DevOpsConfigurer
{
    using Extract.DevOps.ExtractWorkItem;
    using Extract.DevOps.Models;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        //private readonly IExtractWorkItem extractWorkItem;

		public MainWindow(IExtractWorkItem extractWorkItem)
		{
            //this.extractWorkItem = extractWorkItem;
            InitializeComponent();
		}

        public MainWindow()
        {

        }

        private  void Button_Click(object sender, RoutedEventArgs e)
		{
            DevOpsParameters devOpsParameters = new DevOpsParameters
            {
                OrganisationName = "OrganizationRP1",
                ProjectName = "ProjectRP1",
                ProjectTeamName = "ProjectRP1 Team"
            };

            IExtractWorkItem extractWorkItem = new ExtractWorkItem();

            var backlogLevelWorkItems = extractWorkItem.GetBacklogLevelWorkItemsAsync(devOpsParameters);

            var workItems = extractWorkItem.GetWorkItemsAsync(devOpsParameters, backlogLevelWorkItems.Result);

            DevOpsDataWindow devOpsDataWindow = new DevOpsDataWindow(workItems.Result);
            devOpsDataWindow.Show();
        }
    }
}
