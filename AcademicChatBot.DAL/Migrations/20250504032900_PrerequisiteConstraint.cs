using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PrerequisiteConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrerequisiteConstraint_Curriculum_CurriculumId",
                table: "PrerequisiteConstraint");

            migrationBuilder.DropIndex(
                name: "IX_PrerequisiteConstraint_CurriculumId",
                table: "PrerequisiteConstraint");

            migrationBuilder.DropColumn(
                name: "CurriculumId",
                table: "PrerequisiteConstraint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumId",
                table: "PrerequisiteConstraint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteConstraint_CurriculumId",
                table: "PrerequisiteConstraint",
                column: "CurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrerequisiteConstraint_Curriculum_CurriculumId",
                table: "PrerequisiteConstraint",
                column: "CurriculumId",
                principalTable: "Curriculum",
                principalColumn: "CurriculumId");
        }
    }
}
