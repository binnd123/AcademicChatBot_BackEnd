using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Subjects
{
    public class UpdateSubjectRequest
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string DecisionNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public int NoCredit { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int SessionNo { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string SyllabusName { get; set; }
        public string DegreeLevel { get; set; } = "Bachelor";
        public string TimeAllocation { get; set; } = "Study hour (150h) = 45h contact hours + 105h self-study";
        public string Description { get; set; }
        public string StudentTasks { get; set; }
        public double ScoringScale { get; set; } = 10;
        public double MinAvgMarkToPass { get; set; } = 5;
        public string Note { get; set; } = string.Empty;
    }
}
