using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Material
    {
        [Key]
        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; } = null!;
        public string MaterialName { get; set; } = null!;
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
