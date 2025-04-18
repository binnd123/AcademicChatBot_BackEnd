using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class ToolForSubject
    {
        [Key]
        public Guid ToolForSubjectId { get; set; }
        public Guid? ToolId { get; set; }
        [ForeignKey(nameof(ToolId))]
        public Tool? Tool { get; set; }
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
