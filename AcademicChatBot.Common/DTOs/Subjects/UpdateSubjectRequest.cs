using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.DTOs.Subjects
{
    public class UpdateSubjectRequest
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string DecisionNo { get; set; }
        public bool IsActive { get; set; } 
        public bool IsApproved { get; set; } 
        public int NoCredit { get; set; } 
        public DateTime ApprovedDate { get; set; }
        public Guid? CurriculumId { get; set; }
    }
}
