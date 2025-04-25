using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PrerequisiteSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrerequisiteSubject_Subject_SubjectId",
                table: "PrerequisiteSubject");

            migrationBuilder.DropIndex(
                name: "IX_PrerequisiteSubject_SubjectId",
                table: "PrerequisiteSubject");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PrerequisiteSubject");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PrerequisiteSubject");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "PrerequisiteSubject");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PrerequisiteSubject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PrerequisiteSubject",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PrerequisiteSubject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "PrerequisiteSubject",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PrerequisiteSubject",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteSubject_SubjectId",
                table: "PrerequisiteSubject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrerequisiteSubject_Subject_SubjectId",
                table: "PrerequisiteSubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId");
        }
    }
}
