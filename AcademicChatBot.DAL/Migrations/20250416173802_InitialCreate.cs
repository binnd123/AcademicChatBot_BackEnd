using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MajorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Combos",
                columns: table => new
                {
                    ComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComboCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComboName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MajorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.ComboId);
                    table.ForeignKey(
                        name: "FK_Combos_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId");
                });

            migrationBuilder.CreateTable(
                name: "Curriculums",
                columns: table => new
                {
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurriculumName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreRequisite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MajorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculums", x => x.CurriculumId);
                    table.ForeignKey(
                        name: "FK_Curriculums_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId");
                });

            migrationBuilder.CreateTable(
                name: "AIChatLogs",
                columns: table => new
                {
                    AIChatLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastMessageTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIChatLogs", x => x.AIChatLogId);
                    table.ForeignKey(
                        name: "FK_AIChatLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntakeYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MajorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId");
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NoCredit = table.Column<int>(type: "int", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                    table.ForeignKey(
                        name: "FK_Subjects_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "CurriculumId");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    IsBotResponse = table.Column<bool>(type: "bit", nullable: false),
                    AIChatLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AIChatLogs_AIChatLogId",
                        column: x => x.AIChatLogId,
                        principalTable: "AIChatLogs",
                        principalColumn: "AIChatLogId");
                });

            migrationBuilder.CreateTable(
                name: "ComboSubjects",
                columns: table => new
                {
                    ComboSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SemesterNo = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboSubjects", x => x.ComboSubjectId);
                    table.ForeignKey(
                        name: "FK_ComboSubjects_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "ComboId");
                    table.ForeignKey(
                        name: "FK_ComboSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "PrerequisiteSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelationGroup = table.Column<int>(type: "int", nullable: false),
                    ConditionType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrerequisiteSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrerequisiteSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrerequisiteSubjects_Subjects_PrerequisiteSubjectId",
                        column: x => x.PrerequisiteSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_PrerequisiteSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "SubjectInCurriculums",
                columns: table => new
                {
                    SubjectInCurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SemesterNo = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectInCurriculums", x => x.SubjectInCurriculumId);
                    table.ForeignKey(
                        name: "FK_SubjectInCurriculums_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "CurriculumId");
                    table.ForeignKey(
                        name: "FK_SubjectInCurriculums_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "Syllabuss",
                columns: table => new
                {
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyllabusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeAllocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentTasks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tools = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoringScale = table.Column<double>(type: "float", nullable: false),
                    MinAvgMarkToPass = table.Column<double>(type: "float", nullable: false),
                    DecisionNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabuss", x => x.SyllabusId);
                    table.ForeignKey(
                        name: "FK_Syllabuss_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Part = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    CompletionCriteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KnowledgeAndSkill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradingGuide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.AssessmentId);
                    table.ForeignKey(
                        name: "FK_Assessments_Syllabuss_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuss",
                        principalColumn: "SyllabusId");
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainMaterial = table.Column<bool>(type: "bit", nullable: false),
                    IsHardCopy = table.Column<bool>(type: "bit", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_Materials_Syllabuss_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuss",
                        principalColumn: "SyllabusId");
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomes",
                columns: table => new
                {
                    LearningOutcomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PLOName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLODescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CLOName = table.Column<int>(type: "int", nullable: false),
                    CLODetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LODetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomes", x => x.LearningOutcomeId);
                    table.ForeignKey(
                        name: "FK_LearningOutcomes_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "AssessmentId");
                    table.ForeignKey(
                        name: "FK_LearningOutcomes_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "CurriculumId");
                    table.ForeignKey(
                        name: "FK_LearningOutcomes_Syllabuss_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuss",
                        principalColumn: "SyllabusId");
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionNo = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningTeachingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ITU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentMaterials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Download = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentTasks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LearningOutcomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_LearningOutcomes_LearningOutcomeId",
                        column: x => x.LearningOutcomeId,
                        principalTable: "LearningOutcomes",
                        principalColumn: "LearningOutcomeId");
                    table.ForeignKey(
                        name: "FK_Sessions_Syllabuss_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuss",
                        principalColumn: "SyllabusId");
                });

            migrationBuilder.CreateTable(
                name: "ConstructiveQuestions",
                columns: table => new
                {
                    ConstructiveQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructiveQuestions", x => x.ConstructiveQuestionId);
                    table.ForeignKey(
                        name: "FK_ConstructiveQuestions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_ConstructiveQuestions_Syllabuss_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuss",
                        principalColumn: "SyllabusId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIChatLogs_UserId",
                table: "AIChatLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_SyllabusId",
                table: "Assessments",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Combos_MajorId",
                table: "Combos",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboSubjects_ComboId",
                table: "ComboSubjects",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboSubjects_SubjectId",
                table: "ComboSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructiveQuestions_SessionId",
                table: "ConstructiveQuestions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructiveQuestions_SyllabusId",
                table: "ConstructiveQuestions",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_MajorId",
                table: "Curriculums",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_AssessmentId",
                table: "LearningOutcomes",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_CurriculumId",
                table: "LearningOutcomes",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_SyllabusId",
                table: "LearningOutcomes",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SyllabusId",
                table: "Materials",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AIChatLogId",
                table: "Messages",
                column: "AIChatLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteSubjects_PrerequisiteSubjectId",
                table: "PrerequisiteSubjects",
                column: "PrerequisiteSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteSubjects_SubjectId",
                table: "PrerequisiteSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LearningOutcomeId",
                table: "Sessions",
                column: "LearningOutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SyllabusId",
                table: "Sessions",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInCurriculums_CurriculumId",
                table: "SubjectInCurriculums",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInCurriculums_SubjectId",
                table: "SubjectInCurriculums",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CurriculumId",
                table: "Subjects",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuss_SubjectId",
                table: "Syllabuss",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboSubjects");

            migrationBuilder.DropTable(
                name: "ConstructiveQuestions");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PrerequisiteSubjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "SubjectInCurriculums");

            migrationBuilder.DropTable(
                name: "Combos");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "AIChatLogs");

            migrationBuilder.DropTable(
                name: "LearningOutcomes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Syllabuss");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Curriculums");

            migrationBuilder.DropTable(
                name: "Majors");
        }
    }
}
