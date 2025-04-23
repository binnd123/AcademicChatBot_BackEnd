using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint
{
    public class UpdatePrerequisiteConstraintRequest
    {
        public string? PrerequisiteConstraintCode { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? CurriculumId { get; set; }
    }
}
