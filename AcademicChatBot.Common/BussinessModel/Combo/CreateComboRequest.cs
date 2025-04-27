using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Combo
{
    public class CreateComboRequest
    {
        public string ComboCode { get; set; } = null!;
        public string ComboName { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public Guid ProgramId { get; set; }
    }
}
