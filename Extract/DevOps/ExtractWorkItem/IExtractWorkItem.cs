namespace Extract.DevOps.ExtractWorkItem
{
    using Extract.DevOps.Models;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IExtractWorkItem
    {
        Task<BacklogLevelWorkItems> GetBacklogLevelWorkItemsAsync(DevOpsParameters devOpsParameters);

        Task<List<WorkItem>> GetWorkItemsAsync(DevOpsParameters devOpsParameters, BacklogLevelWorkItems backlogLevelWorkItems);
    }
}
