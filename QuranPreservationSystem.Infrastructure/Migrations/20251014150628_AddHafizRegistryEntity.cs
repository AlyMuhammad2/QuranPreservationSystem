using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuranPreservationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHafizRegistryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HafizRegistry",
                columns: table => new
                {
                    HafizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CenterId = table.Column<int>(type: "int", nullable: false),
                    CompletionYear = table.Column<int>(type: "int", nullable: false),
                    CompletedCourses = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CertificatePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CertificateFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificateFileSize = table.Column<long>(type: "bigint", nullable: true),
                    CertificateFileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MemorizationLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CertificateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Supervisor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExaminationCommittee = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HafizRegistry", x => x.HafizId);
                    table.ForeignKey(
                        name: "FK_HafizRegistry_Centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centers",
                        principalColumn: "CenterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistry_CenterId",
                table: "HafizRegistry",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistry_CompletionYear",
                table: "HafizRegistry",
                column: "CompletionYear");

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistry_StudentName",
                table: "HafizRegistry",
                column: "StudentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HafizRegistry");
        }
    }
}
