using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.DAL.Models
{
    public class PrerequisiteSubject
    {
        [Key]
        public Guid Id { get; set; }
        public int RelationGroup { get; set; }
        public ConditionTypeName ConditionType { get; set; } = ConditionTypeName.AND;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
        public Guid? PrerequisiteSubjectId { get; set; }
        [ForeignKey(nameof(PrerequisiteSubjectId))]
        public Subject? PrerequisiteSubjectInfo { get; set; }
        public Guid? PrerequisiteConstraintId { get; set; }
        [ForeignKey(nameof(PrerequisiteConstraintId))]
        public PrerequisiteConstraint? PrerequisiteConstraint { get; set; }
    }
}
