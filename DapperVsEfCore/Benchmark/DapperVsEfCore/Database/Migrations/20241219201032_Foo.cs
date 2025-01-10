using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchy.DapperVsEfCore.Database.Migrations
{
    /// <inheritdoc />
    public partial class Foo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ThumbnailId",
                schema: "DapperVsEfCore",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ThumbnailId",
                schema: "DapperVsEfCore",
                table: "Vehicles",
                column: "ThumbnailId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ThumbnailId",
                schema: "DapperVsEfCore",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ThumbnailId",
                schema: "DapperVsEfCore",
                table: "Vehicles",
                column: "ThumbnailId");
        }
    }
}
