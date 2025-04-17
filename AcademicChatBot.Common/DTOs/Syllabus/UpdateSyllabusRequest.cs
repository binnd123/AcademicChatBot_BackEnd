using System;

namespace AcademicChatBot.Common.DTOs.Syllabus
{
    public class UpdateSyllabusRequest
    {
        public string? SyllabusCode { get; set; }
        public string? SyllabusName { get; set; }
        public string? DegreeLevel { get; set; }
        public string? TimeAllocation { get; set; }
        public string? Description { get; set; }
        public string? StudentTasks { get; set; }
        public string? Tools { get; set; }
        public double? ScoringScale { get; set; }
        public double? MinAvgMarkToPass { get; set; }
        public string? DecisionNo { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? SubjectId { get; set; }
    }
}
