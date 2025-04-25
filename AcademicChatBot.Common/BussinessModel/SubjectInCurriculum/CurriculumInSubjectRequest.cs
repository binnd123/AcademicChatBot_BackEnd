using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.SubjectInCurriculum
{
    public class CurriculumInSubjectRequest
    {
        public Guid CurriculumId { get; set; }
        public int SemesterNo { get; set; }
    }
}
