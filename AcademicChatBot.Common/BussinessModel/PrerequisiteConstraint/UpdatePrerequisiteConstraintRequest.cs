using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint
{
    public class UpdatePrerequisiteConstraintRequest
    {
        public string? PrerequisiteConstraintCode { get; set; }
        public ConditionTypeName GroupCombinationType { get; set; } = ConditionTypeName.OR;
        public Guid? SubjectId { get; set; }
    }
}
