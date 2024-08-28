using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace graduationProject.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderIdToJobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsNegotiable",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProviderId",
                table: "Jobs",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Providers_ProviderId",
                table: "Jobs",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Providers_ProviderId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProviderId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Jobs");

            migrationBuilder.AddColumn<bool>(
                name: "IsNegotiable",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
