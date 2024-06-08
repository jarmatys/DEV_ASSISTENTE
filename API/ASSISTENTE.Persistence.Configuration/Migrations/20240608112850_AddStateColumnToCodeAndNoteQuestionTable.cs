using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASSISTENTE.Persistence.Configuration.Migrations
{
    /// <inheritdoc />
    public partial class AddStateColumnToCodeAndNoteQuestionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "QuestionNotes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "QuestionCodes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "QuestionNotes");

            migrationBuilder.DropColumn(
                name: "State",
                table: "QuestionCodes");
        }
    }
}
