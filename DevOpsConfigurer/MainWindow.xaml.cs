namespace DevOpsConfigurer
{    
    using System.Windows;
    using Extract.DevOps.ExtractWorkItem;
    using Extract.DevOps.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        public MainWindow()
        {
            InitializeComponent();
            IExtractWorkItem extractWorkItem = new ExtractWorkItem();
            organisation.Text = "OrganizationRP1";
            
            DevOpsParameters devOpsParameters = new DevOpsParameters
            {
                OrganisationName = organisation.Text,
                ProjectName = "ProjectRP1",
                ProjectTeamName = "ProjectRP1 Team"
            };            

            projects.ItemsSource = extractWorkItem.GetProjectsByOrganisation(devOpsParameters).Result;
            projects.Text = "Please select";
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
		{
            if (projects.SelectedValue != null)
            {
                IExtractWorkItem extractWorkItem = new ExtractWorkItem();
                DevOpsParameters devOpsParameters = new DevOpsParameters
                {
                    OrganisationName = organisation.Text,
                    ProjectName = projects.SelectedValue.ToString(),
                    ProjectTeamName = "ProjectRP1 Team"
                };

                var backlogLevelWorkItems = extractWorkItem.GetBacklogLevelWorkItemsAsync(devOpsParameters);

                var workItems = extractWorkItem.GetWorkItemsAsync(devOpsParameters, backlogLevelWorkItems.Result);

                DevOpsDataWindow devOpsDataWindow = new DevOpsDataWindow(workItems.Result);
                devOpsDataWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select project", "Error");
            }
        }
    }
}
