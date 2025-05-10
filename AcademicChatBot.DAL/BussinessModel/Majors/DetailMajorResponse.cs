using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.DAL.BussinessModel.Majors
{
    public class DetailMajorResponse
    {
        public Guid MajorId { get; set; }
        public string MajorCode { get; set; } 
        public string MajorName { get; set; } 
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Curriculum> Curriculums { get; set; }
    }
}
