using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.PrerequisiteSubject
{
    public class PrerequisiteSubjectsToPrerequisiteConstrainRequest
    {
        public int RelationGroup { get; set; }
        public ConditionTypeName ConditionType { get; set; } = ConditionTypeName.AND;
        public Guid? SubjectId { get; set; }
    }
}
