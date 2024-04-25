using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASSISTENTE.Persistence.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedConnectionIdColumnToQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Questions");
        }
    }
}
