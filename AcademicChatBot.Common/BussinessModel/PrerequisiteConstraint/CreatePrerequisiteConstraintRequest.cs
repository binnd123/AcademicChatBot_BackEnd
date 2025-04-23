using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint
{
    public class CreatePrerequisiteConstraintRequest
    {
        public string PrerequisiteConstraintCode { get; set; } = null!;
        public Guid? SubjectId { get; set; }
        public Guid? CurriculumId { get; set; }
    }
}
