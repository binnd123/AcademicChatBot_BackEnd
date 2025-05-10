using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.DAL.Models
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public int? IntakeYear { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public Guid? MajorId { get; set; }
        [ForeignKey(nameof(MajorId))]
        public Major? Major { get; set; }
    }
}
