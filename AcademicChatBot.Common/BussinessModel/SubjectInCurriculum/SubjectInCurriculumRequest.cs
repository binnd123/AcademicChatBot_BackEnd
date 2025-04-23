using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.SubjectInCurriculum
{
    public class SubjectInCurriculumRequest
    {
        public Guid SubjectId { get; set; }
        public int SemesterNo { get; set; }
    }
}
