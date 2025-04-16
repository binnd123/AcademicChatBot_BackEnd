using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class SubjectInCurriculum
    {
        [Key]
        public Guid SubjectInCurriculumId { get; set; }
        public int SemesterNo{ get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
        public Guid? CurriculumId { get; set; }
        [ForeignKey(nameof(CurriculumId))]
        public Curriculum? Curriculum { get; set; }
    }
}
