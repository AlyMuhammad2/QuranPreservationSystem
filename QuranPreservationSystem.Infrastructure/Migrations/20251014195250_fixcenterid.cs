using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuranPreservationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixcenterid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HafizRegistry_Centers_CenterId",
                table: "HafizRegistry");

            migrationBuilder.AlterColumn<int>(
                name: "CenterId",
                table: "HafizRegistry",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HafizRegistry_Centers_CenterId",
                table: "HafizRegistry",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "CenterId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HafizRegistry_Centers_CenterId",
                table: "HafizRegistry");

            migrationBuilder.AlterColumn<int>(
                name: "CenterId",
                table: "HafizRegistry",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HafizRegistry_Centers_CenterId",
                table: "HafizRegistry",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "CenterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
