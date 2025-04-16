using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Subject
    {
        [Key]
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string DecisionNo { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } =false;
        public int NoCredit { get; set; } = 3;
        public DateTime ApprovedDate { get; set; }
        public Guid? CurriculumId { get; set; }
        [ForeignKey(nameof(CurriculumId))]
        public Curriculum? Curriculum { get; set; }
    }
}
