using System;

namespace AcademicChatBot.Common.BussinessModel.Curriculum
{
    public class UpdateCurriculumRequest
    {
        public string? CurriculumCode { get; set; }
        public string? CurriculumName { get; set; }
        public string? Description { get; set; }
        public string? DecisionNo { get; set; }
        public string? PreRequisite { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public Guid? MajorId { get; set; }
    }
}
