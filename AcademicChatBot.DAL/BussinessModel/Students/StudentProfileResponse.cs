using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.DAL.BussinessModel.Students
{
    public class StudentProfileResponse
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } 
        public string FullName { get; set; } 
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public int? IntakeYear { get; set; }
        public GenderType Gender { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } 
        public User? User { get; set; }
        public Major? Major { get; set; }
    }
}
