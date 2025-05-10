using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CloAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLearningOutcome_Assessment_AssessmentId",
                table: "CourseLearningOutcome");

            migrationBuilder.DropIndex(
                name: "IX_CourseLearningOutcome_AssessmentId",
                table: "CourseLearningOutcome");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "CourseLearningOutcome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssessmentId",
                table: "CourseLearningOutcome",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLearningOutcome_AssessmentId",
                table: "CourseLearningOutcome",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLearningOutcome_Assessment_AssessmentId",
                table: "CourseLearningOutcome",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "AssessmentId");
        }
    }
}
