using System;

namespace AcademicChatBot.Common.BussinessModel.Curriculum
{
    public class CreateCurriculumRequest
    {
        public string CurriculumCode { get; set; } = null!;
        public string CurriculumName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DecisionNo { get; set; } = null!;
        public string PreRequisite { get; set; } = "None";
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public Guid? MajorId { get; set; }
    }
}
