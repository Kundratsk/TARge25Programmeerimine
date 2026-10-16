using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKindergartenIdToFileToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KindergartenId",
                table: "FileToDatabases",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KindergartenId",
                table: "FileToDatabases");
        }
    }
}
