namespace Load.DevOpsToDevOps.DevOpsWorkItem
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Load.DevOpsToDevOps.Models;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public interface IDevOpsWorkItem
    {
        void CreateWorkItem(DevOpsParameters devOpsParameters, WorkItem workItem);
    }
}
