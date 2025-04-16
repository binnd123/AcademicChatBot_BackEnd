using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class ComboSubject
    {
        [Key]
        public Guid ComboSubjectId { get; set; }
        public int SemesterNo { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
        public Guid? ComboId { get; set; }
        [ForeignKey(nameof(ComboId))]
        public Combo? Combo { get; set; }
    }
}
