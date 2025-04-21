using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Tools
{
    public class CreateToolRequest
    {
        public string ToolCode { get; set; } = null!;
        public string ToolName { get; set; } = string.Empty;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}

