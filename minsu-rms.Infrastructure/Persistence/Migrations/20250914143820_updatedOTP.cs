using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Middlename",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MonthOfBirth",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Program",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentID",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YearLevel",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YearOfBirth",
                table: "OTPRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "HomeAddress",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "Middlename",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "MonthOfBirth",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "YearLevel",
                table: "OTPRequests");

            migrationBuilder.DropColumn(
                name: "YearOfBirth",
                table: "OTPRequests");
        }
    }
}
