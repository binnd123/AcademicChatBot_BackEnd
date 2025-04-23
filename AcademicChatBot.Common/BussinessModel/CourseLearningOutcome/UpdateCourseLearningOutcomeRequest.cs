using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.CourseLearningOutcome
{
    public class UpdateCourseLearningOutcomeRequest
    {
        public string? CourseLearningOutcomeCode { get; set; }
        public string? CourseLearningOutcomeName { get; set; }
        public string? CourseLearningOutcomeDetail { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? AssessmentId { get; set; }
    }
}
