using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingDomainNameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_studentUserData",
                table: "studentUserData");

            migrationBuilder.RenameTable(
                name: "studentUserData",
                newName: "studentUserAccount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentUserAccount",
                table: "studentUserAccount",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_studentUserAccount",
                table: "studentUserAccount");

            migrationBuilder.RenameTable(
                name: "studentUserAccount",
                newName: "studentUserData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentUserData",
                table: "studentUserData",
                column: "Id");
        }
    }
}
