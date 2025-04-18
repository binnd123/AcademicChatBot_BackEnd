using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class ProgramingOutcome
    {
        [Key]
        public Guid ProgramingOutcomeId { get; set; }
        public string ProgramingOutcomeCode { get; set; } = null!;
        public string ProgramingOutcomeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? ProgramId { get; set; }
        [ForeignKey(nameof(ProgramId))]
        public Program? Program { get; set; }
    }
}
