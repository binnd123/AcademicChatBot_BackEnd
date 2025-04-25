using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Material
{
    public class CreateMaterialRequest
    {
        public string MaterialName { get; set; } = null!;
        public string MaterialCode { get; set; } = null!;
        public string MaterialDescription { get; set; } = null!;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Edition { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public bool IsMainMaterial { get; set; } = false;
        public bool IsHardCopy { get; set; } = false;
        public bool IsOnline { get; set; } = false;
        public string Note { get; set; } = string.Empty;
        public Guid? SubjectId { get; set; }
    }
}
