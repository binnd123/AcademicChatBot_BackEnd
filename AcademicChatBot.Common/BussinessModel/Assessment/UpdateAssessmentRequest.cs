using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Assessment
{
    public class UpdateAssessmentRequest
    {
        public string? Category { get; set; }
        public string? Type { get; set; }
        public int? Part { get; set; }
        public double? Weight { get; set; }
        public string? CompletionCriteria { get; set; }
        public string? Duration { get; set; }
        public string? QuestionType { get; set; }
        public string? NoQuestion { get; set; }
        public string? KnowledgeAndSkill { get; set; }
        public string? GradingGuide { get; set; }
        public string? Note { get; set; }
        public Guid? SubjectId { get; set; }
    }
}
