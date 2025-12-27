using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedjwtrefreshtokenrolescolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "RefreshTokens");
        }
    }
}
