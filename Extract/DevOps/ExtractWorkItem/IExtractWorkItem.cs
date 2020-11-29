namespace Extract.DevOps.ExtractWorkItem
{    
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extract.DevOps.Models;
    using Microsoft.TeamFoundation.Core.WebApi;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public interface IExtractWorkItem
    {
        Task<List<string>> GetProjectsByOrganisation(DevOpsParameters devOpsParameters);

        Task<BacklogLevelWorkItems> GetBacklogLevelWorkItemsAsync(DevOpsParameters devOpsParameters);

        Task<List<WorkItem>> GetWorkItemsAsync(DevOpsParameters devOpsParameters, BacklogLevelWorkItems backlogLevelWorkItems);
    }
}
