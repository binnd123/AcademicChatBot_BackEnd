using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Assessment
{
    public class CreateAssessmentRequest
    {
        public string Category { get; set; } = null!;
        public string Type { get; set; } = "on-going";
        public int Part { get; set; }
        public double Weight { get; set; }
        public string CompletionCriteria { get; set; } = ">0";
        public string Duration { get; set; } = null!;
        public string QuestionType { get; set; } = string.Empty;
        public string NoQuestion { get; set; } = string.Empty;
        public string KnowledgeAndSkill { get; set; } = string.Empty;
        public string GradingGuide { get; set; } = string.Empty;
        public string Note { get; set; } = null!;
        public Guid SubjectId { get; set; }
    }
}
