using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.DAL.BussinessModel.Subjects
{
    public class DetailSubjectResponse
    {
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int SessionNo { get; set; }
        public string DecisionNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int NoCredit { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string SyllabusName { get; set; }
        public string DegreeLevel { get; set; }
        public string TimeAllocation { get; set; }
        public string Description { get; set; }
        public string StudentTasks { get; set; }
        public double ScoringScale { get; set; }
        public double MinAvgMarkToPass { get; set; }
        public string Note { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<CourseLearningOutcome> CourseLearningOutcomes { get; set; }
        public List<Tool> Tools { get; set; }
        public List<Material> Materials { get; set; }
        public object PrerequisiteSubjects { get; set; }
    }
}
