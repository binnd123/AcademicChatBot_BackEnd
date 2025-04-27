using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Combo
{
    public class UpdateComboRequest
    {
        public string? ComboCode { get; set; }
        public string? ComboName { get; set; }
        public string? Note { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public Guid? ProgramId { get; set; }
    }
}
