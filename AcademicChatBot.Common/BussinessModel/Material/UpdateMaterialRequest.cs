using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Material
{
    public class UpdateMaterialRequest
    {
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialDescription { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? Edition { get; set; }
        public string? ISBN { get; set; }
        public bool? IsMainMaterial { get; set; }
        public bool? IsHardCopy { get; set; }
        public bool? IsOnline { get; set; }
        public string? Note { get; set; }
        public Guid? SubjectId { get; set; }
    }
}
