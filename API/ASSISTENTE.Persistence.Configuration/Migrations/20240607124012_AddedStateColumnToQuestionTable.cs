using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASSISTENTE.Persistence.Configuration.Migrations
{
    /// <inheritdoc />
    public partial class AddedStateColumnToQuestionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Questions");
        }
    }
}
