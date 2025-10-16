using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuranPreservationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class logsmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CenterLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CenterName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CenterLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentInfo = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HafizRegistryLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HafizName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HafizRegistryLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CenterLogs_ActionType_Timestamp",
                table: "CenterLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CenterLogs_RecordId_Timestamp",
                table: "CenterLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CenterLogs_Timestamp",
                table: "CenterLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_CenterLogs_UserId_Timestamp",
                table: "CenterLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLogs_ActionType_Timestamp",
                table: "CourseLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLogs_RecordId_Timestamp",
                table: "CourseLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLogs_Timestamp",
                table: "CourseLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLogs_UserId_Timestamp",
                table: "CourseLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentLogs_ActionType_Timestamp",
                table: "EnrollmentLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentLogs_RecordId_Timestamp",
                table: "EnrollmentLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentLogs_Timestamp",
                table: "EnrollmentLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentLogs_UserId_Timestamp",
                table: "EnrollmentLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamLogs_ActionType_Timestamp",
                table: "ExamLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamLogs_RecordId_Timestamp",
                table: "ExamLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamLogs_Timestamp",
                table: "ExamLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ExamLogs_UserId_Timestamp",
                table: "ExamLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistryLogs_ActionType_Timestamp",
                table: "HafizRegistryLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistryLogs_RecordId_Timestamp",
                table: "HafizRegistryLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistryLogs_Timestamp",
                table: "HafizRegistryLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_HafizRegistryLogs_UserId_Timestamp",
                table: "HafizRegistryLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_ActionType_Timestamp",
                table: "StudentLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_RecordId_Timestamp",
                table: "StudentLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_Timestamp",
                table: "StudentLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_UserId_Timestamp",
                table: "StudentLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLogs_ActionType_Timestamp",
                table: "TeacherLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLogs_RecordId_Timestamp",
                table: "TeacherLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLogs_Timestamp",
                table: "TeacherLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLogs_UserId_Timestamp",
                table: "TeacherLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_ActionType_Timestamp",
                table: "UserLogs",
                columns: new[] { "ActionType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_RecordId_Timestamp",
                table: "UserLogs",
                columns: new[] { "RecordId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_Timestamp",
                table: "UserLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId_Timestamp",
                table: "UserLogs",
                columns: new[] { "UserId", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CenterLogs");

            migrationBuilder.DropTable(
                name: "CourseLogs");

            migrationBuilder.DropTable(
                name: "EnrollmentLogs");

            migrationBuilder.DropTable(
                name: "ExamLogs");

            migrationBuilder.DropTable(
                name: "HafizRegistryLogs");

            migrationBuilder.DropTable(
                name: "StudentLogs");

            migrationBuilder.DropTable(
                name: "TeacherLogs");

            migrationBuilder.DropTable(
                name: "UserLogs");
        }
    }
}
