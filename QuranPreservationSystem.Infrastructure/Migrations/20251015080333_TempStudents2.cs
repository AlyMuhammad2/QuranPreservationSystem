using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuranPreservationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TempStudents2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "TempStudentImports");

            migrationBuilder.DropColumn(
                name: "MedicalInfo",
                table: "TempStudentImports");

            migrationBuilder.DropColumn(
                name: "MemorizationLevel",
                table: "TempStudentImports");

            migrationBuilder.DropColumn(
                name: "ParentPhoneNumber",
                table: "TempStudentImports");

            migrationBuilder.DropColumn(
                name: "RelationToGuardian",
                table: "TempStudentImports");

            migrationBuilder.DropColumn(
                name: "School",
                table: "TempStudentImports");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TempStudentImports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TempStudentImports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "TempStudentImports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalInfo",
                table: "TempStudentImports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemorizationLevel",
                table: "TempStudentImports",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPhoneNumber",
                table: "TempStudentImports",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelationToGuardian",
                table: "TempStudentImports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School",
                table: "TempStudentImports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
