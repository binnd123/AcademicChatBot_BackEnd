﻿// <auto-generated />
using System;
using AcademicChatBot.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    [DbContext(typeof(AcademicChatBotDBContext))]
    [Migration("20250417070850_Token")]
    partial class Token
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AcademicChatBot.DAL.Models.AIChatLog", b =>
                {
                    b.Property<Guid>("AIChatLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastMessageTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AIChatLogId");

                    b.HasIndex("UserId");

                    b.ToTable("AIChatLogs");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Assessment", b =>
                {
                    b.Property<Guid>("AssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompletionCriteria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GradingGuide")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("KnowledgeAndSkill")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoQuestion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Part")
                        .HasColumnType("int");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SyllabusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("AssessmentId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Combo", b =>
                {
                    b.Property<Guid>("ComboId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ComboCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ComboName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("MajorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ComboId");

                    b.HasIndex("MajorId");

                    b.ToTable("Combos");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.ComboSubject", b =>
                {
                    b.Property<Guid>("ComboSubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ComboId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SemesterNo")
                        .HasColumnType("int");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ComboSubjectId");

                    b.HasIndex("ComboId");

                    b.HasIndex("SubjectId");

                    b.ToTable("ComboSubjects");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.ConstructiveQuestion", b =>
                {
                    b.Property<Guid>("ConstructiveQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SyllabusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConstructiveQuestionId");

                    b.HasIndex("SessionId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("ConstructiveQuestions");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Curriculum", b =>
                {
                    b.Property<Guid>("CurriculumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurriculumCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurriculumName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DecisionNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("MajorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PreRequisite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CurriculumId");

                    b.HasIndex("MajorId");

                    b.ToTable("Curriculums");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.LearningOutcome", b =>
                {
                    b.Property<Guid>("LearningOutcomeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssessmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CLODetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CLOName")
                        .HasColumnType("int");

                    b.Property<Guid?>("CurriculumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LODetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LOName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLODescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLOName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SyllabusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LearningOutcomeId");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("CurriculumId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("LearningOutcomes");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Major", b =>
                {
                    b.Property<Guid>("MajorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MajorCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MajorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("MajorId");

                    b.ToTable("Majors");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Material", b =>
                {
                    b.Property<Guid>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Edition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHardCopy")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMainMaterial")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<string>("MaterialCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaterialDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SyllabusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MaterialId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Message", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AIChatLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsBotResponse")
                        .HasColumnType("bit");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("AIChatLogId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Notification", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.PrerequisiteSubject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConditionType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("PrerequisiteSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RelationGroup")
                        .HasColumnType("int");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PrerequisiteSubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("PrerequisiteSubjects");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Download")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ITU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("LearningOutcomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LearningTeachingType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SessionNo")
                        .HasColumnType("int");

                    b.Property<string>("StudentMaterials")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentTasks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SyllabusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URLs")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SessionId");

                    b.HasIndex("LearningOutcomeId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IntakeYear")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("MajorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StudentId");

                    b.HasIndex("MajorId");

                    b.HasIndex("UserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Subject", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CurriculumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DecisionNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("NoCredit")
                        .HasColumnType("int");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.HasIndex("CurriculumId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.SubjectInCurriculum", b =>
                {
                    b.Property<Guid>("SubjectInCurriculumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CurriculumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("SemesterNo")
                        .HasColumnType("int");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SubjectInCurriculumId");

                    b.HasIndex("CurriculumId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectInCurriculums");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Syllabus", b =>
                {
                    b.Property<Guid>("SyllabusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DecisionNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DegreeLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double>("MinAvgMarkToPass")
                        .HasColumnType("float");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ScoringScale")
                        .HasColumnType("float");

                    b.Property<string>("StudentTasks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SyllabusCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SyllabusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeAllocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tools")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SyllabusId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Syllabuss");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpiredRefreshToken")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.AIChatLog", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Assessment", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Combo", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Major", "Major")
                        .WithMany()
                        .HasForeignKey("MajorId");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.ComboSubject", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Combo", "Combo")
                        .WithMany()
                        .HasForeignKey("ComboId");

                    b.HasOne("AcademicChatBot.DAL.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.Navigation("Combo");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.ConstructiveQuestion", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId");

                    b.HasOne("AcademicChatBot.DAL.Models.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId");

                    b.Navigation("Session");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Curriculum", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Major", "Major")
                        .WithMany()
                        .HasForeignKey("MajorId");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.LearningOutcome", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Assessment", "Assessment")
                        .WithMany()
                        .HasForeignKey("AssessmentId");

                    b.HasOne("AcademicChatBot.DAL.Models.Curriculum", "Curriculum")
                        .WithMany()
                        .HasForeignKey("CurriculumId");

                    b.HasOne("AcademicChatBot.DAL.Models.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId");

                    b.Navigation("Assessment");

                    b.Navigation("Curriculum");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Material", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Message", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.AIChatLog", "AIChatLog")
                        .WithMany()
                        .HasForeignKey("AIChatLogId");

                    b.Navigation("AIChatLog");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Notification", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.PrerequisiteSubject", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Subject", "PrerequisiteSubjectInfo")
                        .WithMany()
                        .HasForeignKey("PrerequisiteSubjectId");

                    b.HasOne("AcademicChatBot.DAL.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.Navigation("PrerequisiteSubjectInfo");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Session", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.LearningOutcome", "LearningOutcome")
                        .WithMany()
                        .HasForeignKey("LearningOutcomeId");

                    b.HasOne("AcademicChatBot.DAL.Models.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId");

                    b.Navigation("LearningOutcome");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Student", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Major", "Major")
                        .WithMany()
                        .HasForeignKey("MajorId");

                    b.HasOne("AcademicChatBot.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Major");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Subject", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Curriculum", "Curriculum")
                        .WithMany()
                        .HasForeignKey("CurriculumId");

                    b.Navigation("Curriculum");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.SubjectInCurriculum", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Curriculum", "Curriculum")
                        .WithMany()
                        .HasForeignKey("CurriculumId");

                    b.HasOne("AcademicChatBot.DAL.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.Navigation("Curriculum");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("AcademicChatBot.DAL.Models.Syllabus", b =>
                {
                    b.HasOne("AcademicChatBot.DAL.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.Navigation("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}
