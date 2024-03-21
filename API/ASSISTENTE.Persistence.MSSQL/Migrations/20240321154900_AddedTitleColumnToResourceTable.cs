using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASSISTENTE.Persistence.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleColumnToResourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Resources");
        }
    }
}
