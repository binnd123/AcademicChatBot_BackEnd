using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.ComboSubject
{
    public class SubjectInComboRequest
    {
        public Guid SubjectId { get; set; }
        public int SemesterNo { get; set; }
        public string Note { get; set; } = "None";
    }
}
