using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.DAL.BussinessModel.Curriculums
{
    public class DetailCurriculumResponse
    {
        public Guid CurriculumId { get; set; }
        public string CurriculumCode { get; set; } 
        public string CurriculumName { get; set; } 
        public string Description { get; set; } 
        public string DecisionNo { get; set; } 
        public string PreRequisite { get; set; }
        public bool IsActive { get; set; } 
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Major? Major { get; set; }
        public Program? Program { get; set; }
        public List<ProgramingLearningOutcome> ProgramingLearningOutcomes { get; set; }
    }
}
