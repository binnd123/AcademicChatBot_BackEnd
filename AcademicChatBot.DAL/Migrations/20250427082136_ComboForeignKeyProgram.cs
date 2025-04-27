using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicChatBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ComboForeignKeyProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combo_Major_MajorId",
                table: "Combo");

            migrationBuilder.RenameColumn(
                name: "MajorId",
                table: "Combo",
                newName: "ProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Combo_MajorId",
                table: "Combo",
                newName: "IX_Combo_ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Combo_Program_ProgramId",
                table: "Combo",
                column: "ProgramId",
                principalTable: "Program",
                principalColumn: "ProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combo_Program_ProgramId",
                table: "Combo");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Combo",
                newName: "MajorId");

            migrationBuilder.RenameIndex(
                name: "IX_Combo_ProgramId",
                table: "Combo",
                newName: "IX_Combo_MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Combo_Major_MajorId",
                table: "Combo",
                column: "MajorId",
                principalTable: "Major",
                principalColumn: "MajorId");
        }
    }
}
