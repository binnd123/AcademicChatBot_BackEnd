using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.DAL.BussinessModel.Programs
{
    public class DetailProgramResponse
    {
        public Guid ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; } 
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Combo> Combos { get; set; }
        public List<ProgramingOutcome> ProgramingOutcomes { get; set; }
        public List<Curriculum> Curriculums { get; set; }
    }
}
