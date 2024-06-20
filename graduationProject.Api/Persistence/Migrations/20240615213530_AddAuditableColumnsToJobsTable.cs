using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace graduationProject.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableColumnsToJobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CreatedById",
                table: "Jobs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UpdatedById",
                table: "Jobs",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_CreatedById",
                table: "Jobs",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UpdatedById",
                table: "Jobs",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_CreatedById",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UpdatedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CreatedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UpdatedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Jobs");
        }
    }
}
