using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademicChatBot.DAL.DBContext
{
    public class AcademicChatBotDBContext : DbContext
    {
        protected AcademicChatBotDBContext()
        {
        }
        public AcademicChatBotDBContext(DbContextOptions<AcademicChatBotDBContext> options) : base(options)
        {
        }
        
        public DbSet<Models.User> User { get; set; } = null!;
        public DbSet<Models.CourseLearningOutcome> CourseLearningOutcome { get; set; } = null!;
        public DbSet<Models.SubjectInCurriculum> SubjectInCurriculum { get; set; } = null!;
        public DbSet<Models.Subject> Subject { get; set; } = null!;
        public DbSet<Models.Student> Student { get; set; } = null!;
        public DbSet<Models.Tool> Tool { get; set; } = null!;
        public DbSet<Models.PrerequisiteSubject> PrerequisiteSubject { get; set; } = null!;
        public DbSet<Models.PrerequisiteConstraint> PrerequisiteConstraint { get; set; } = null!;
        public DbSet<Models.Notification> Notification { get; set; } = null!;
        public DbSet<Models.Message> Message { get; set; } = null!;
        public DbSet<Models.Material> Material { get; set; } = null!;
        public DbSet<Models.Major> Major { get; set; } = null!;
        public DbSet<Models.ToolForSubject> ToolForSubject { get; set; } = null!;
        public DbSet<Models.Curriculum> Curriculum { get; set; } = null!;
        public DbSet<Models.ProgramingLearningOutcome> ProgramingLearningOutcome { get; set; } = null!;
        public DbSet<Models.ProgramingOutcome> ProgramingOutcome { get; set; } = null!;
        public DbSet<Models.POMappingPLO> POMappingPLO { get; set; } = null!;
        public DbSet<Models.Program> Program { get; set; } = null!;
        public DbSet<Models.ComboSubject> ComboSubject { get; set; } = null!;
        public DbSet<Models.Combo> Combo { get; set; } = null!;
        public DbSet<Models.Assessment> Assessment { get; set; } = null!;
        public DbSet<Models.AIChatLog> AIChatLog { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=AcademicChatBotDB");

    }
}
