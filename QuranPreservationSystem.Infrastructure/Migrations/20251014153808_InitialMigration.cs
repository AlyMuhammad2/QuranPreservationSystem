using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuranPreservationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateDate",
                table: "HafizRegistry");

            migrationBuilder.DropColumn(
                name: "ExaminationCommittee",
                table: "HafizRegistry");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "HafizRegistry");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "HafizRegistry");

            migrationBuilder.DropColumn(
                name: "Supervisor",
                table: "HafizRegistry");

            migrationBuilder.AlterColumn<int>(
                name: "MemorizationLevel",
                table: "HafizRegistry",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MemorizationLevel",
                table: "HafizRegistry",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificateDate",
                table: "HafizRegistry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExaminationCommittee",
                table: "HafizRegistry",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Grade",
                table: "HafizRegistry",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "HafizRegistry",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supervisor",
                table: "HafizRegistry",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
