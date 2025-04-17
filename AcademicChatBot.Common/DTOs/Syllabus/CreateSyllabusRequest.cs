using System;

namespace AcademicChatBot.Common.DTOs.Syllabus
{
    public class CreateSyllabusRequest
    {
        public string SyllabusCode { get; set; }
        public string SyllabusName { get; set; }
        public string DegreeLevel { get; set; } = "Bachelor";
        public string TimeAllocation { get; set; } = "Study hour (150h) = 45h contact hours + 105h self-study";
        public string Description { get; set; }
        public string StudentTasks { get; set; }
        public string Tools { get; set; }
        public double ScoringScale { get; set; } = 10;
        public double MinAvgMarkToPass { get; set; } = 5;
        public string DecisionNo { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public DateTime ApprovedDate { get; set; }
        public Guid? SubjectId { get; set; }
    }
}
