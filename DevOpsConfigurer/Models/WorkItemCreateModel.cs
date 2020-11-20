using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsConfigurer.Models
{
    public class WorkItemCreateModel
    {
        public string op = "add";

        public string path { get; set; }

        public string from { get; set; }

        public string value { get; set; }
    }
}
