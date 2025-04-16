using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime DOB { get; set; }
        public DateTime IntakeYear { get; set; }
        public bool Gender { get; set; }
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public Guid? MajorId { get; set; }
        [ForeignKey(nameof(MajorId))]
        public Major? Major { get; set; }
    }
}
