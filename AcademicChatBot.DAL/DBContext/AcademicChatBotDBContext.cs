﻿using System;
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
        
        public DbSet<Models.User> Users { get; set; } = null!;
        public DbSet<Models.Syllabus> Syllabuss { get; set; } = null!;
        public DbSet<Models.SubjectInCurriculum> SubjectInCurriculums { get; set; } = null!;
        public DbSet<Models.Subject> Subjects { get; set; } = null!;
        public DbSet<Models.Student> Students { get; set; } = null!;
        public DbSet<Models.Session> Sessions { get; set; } = null!;
        public DbSet<Models.PrerequisiteSubject> PrerequisiteSubjects { get; set; } = null!;
        public DbSet<Models.Notification> Notifications { get; set; } = null!;
        public DbSet<Models.Message> Messages { get; set; } = null!;
        public DbSet<Models.Material> Materials { get; set; } = null!;
        public DbSet<Models.Major> Majors { get; set; } = null!;
        public DbSet<Models.LearningOutcome> LearningOutcomes { get; set; } = null!;
        public DbSet<Models.Curriculum> Curriculums { get; set; } = null!;
        public DbSet<Models.ConstructiveQuestion> ConstructiveQuestions { get; set; } = null!;
        public DbSet<Models.ComboSubject> ComboSubjects { get; set; } = null!;
        public DbSet<Models.Combo> Combos { get; set; } = null!;
        public DbSet<Models.Assessment> Assessments { get; set; } = null!;
        public DbSet<Models.AIChatLog> AIChatLogs { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=AcademicChatBotDB");

    }
}
