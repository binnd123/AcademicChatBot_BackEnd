using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DropCurriculumInSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Curriculum_CurriculumId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Subject_CurriculumId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "CurriculumId",
                table: "Subject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumId",
                table: "Subject",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subject_CurriculumId",
                table: "Subject",
                column: "CurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Curriculum_CurriculumId",
                table: "Subject",
                column: "CurriculumId",
                principalTable: "Curriculum",
                principalColumn: "CurriculumId");
        }
    }
}
