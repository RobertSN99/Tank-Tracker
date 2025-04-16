using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCreatedByAndUpdatedByFieldsFromTank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_AspNetUsers_CreatedById",
                table: "Tanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_AspNetUsers_UpdatedById",
                table: "Tanks");

            migrationBuilder.DropIndex(
                name: "IX_Tanks_CreatedById",
                table: "Tanks");

            migrationBuilder.DropIndex(
                name: "IX_Tanks_UpdatedById",
                table: "Tanks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Tanks");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Tanks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Tanks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Tanks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_CreatedById",
                table: "Tanks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_UpdatedById",
                table: "Tanks",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_AspNetUsers_CreatedById",
                table: "Tanks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_AspNetUsers_UpdatedById",
                table: "Tanks",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
