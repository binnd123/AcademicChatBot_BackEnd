using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Accounts
{
    public class AdminReportResponse
    {
        public int TotalStudents { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalPrograms { get; set; }
        public int TotalCurriculums { get; set; }
        public int TotalMajors { get; set; }
        public int TotalTools { get; set; }
        public int TotalCombos { get; set; }
        public int TotalMaterials { get; set; }
        public int TotalMessages { get; set; }
        public int TotalAssessments { get; set; }
        public int TotalPLOs { get; set; }
        public int TotalPOs { get; set; }
        public int TotalCLOs { get; set; }
    }
}
