using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.CourseLearningOutcome
{
    public class CreateCourseLearningOutcomeRequest
    {
        public string CourseLearningOutcomeCode { get; set; } = null!;
        public string CourseLearningOutcomeName { get; set; } = null!;
        public string CourseLearningOutcomeDetail { get; set; } = null!;
        public Guid SubjectId { get; set; }
        public Guid AssessmentId { get; set; }
    }
}
