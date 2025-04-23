using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.PrerequisiteSubject
{
    public class UpdatePrerequisiteSubjectRequest
    {
        public int? RelationGroup { get; set; }
        public ConditionTypeName? ConditionType { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? PrerequisiteSubjectId { get; set; }
        public Guid? PrerequisiteConstraintId { get; set; }
    }
}
