using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.AIChatLogs
{
    public class AcademicContext
    {
        public object Majors { get; set; }
        public object Curriculums { get; set; }
        public object Subjects { get; set; }
        public object PrerequisiteSubjects { get; set; }
        public object SubjectInCurriculums { get; set; }
        public object Programs { get; set; }
        public object Combos { get; set; }
        public object ComboSubjects { get; set; }
    }
}
