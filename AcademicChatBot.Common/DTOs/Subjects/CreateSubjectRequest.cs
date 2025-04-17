using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.DTOs.Subjects
{
    public class CreateSubjectRequest
    {
        public string SubjectCode { get; set; } 
        public string SubjectName { get; set; } 
        public string DecisionNo { get; set; } 
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public int NoCredit { get; set; } = 3;
        public DateTime ApprovedDate { get; set; } = DateTime.Now;
        public Guid? CurriculumId { get; set; }
    }
}
