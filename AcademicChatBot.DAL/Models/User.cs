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
    public class User 
    {
        [Key]
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public RoleName Role { get; set; } = RoleName.Student;
        public string? RefreshToken { get; set; }
        public DateTime? ExpiredRefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
