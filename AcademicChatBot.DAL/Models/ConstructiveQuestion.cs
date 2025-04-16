using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class ConstructiveQuestion
    {
        [Key]
        public Guid ConstructiveQuestionId { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public Guid? SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session? Session { get; set; }
        public Guid? SyllabusId { get; set; }
        [ForeignKey(nameof(SyllabusId))]
        public Syllabus? Syllabus { get; set; }
    }
}
