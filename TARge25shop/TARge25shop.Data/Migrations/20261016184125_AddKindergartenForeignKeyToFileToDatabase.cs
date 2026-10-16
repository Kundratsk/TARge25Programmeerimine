using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKindergartenForeignKeyToFileToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FileToDatabases_KindergartenId",
                table: "FileToDatabases",
                column: "KindergartenId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileToDatabases_Kindergarten_KindergartenId",
                table: "FileToDatabases",
                column: "KindergartenId",
                principalTable: "Kindergarten",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileToDatabases_Kindergarten_KindergartenId",
                table: "FileToDatabases");

            migrationBuilder.DropIndex(
                name: "IX_FileToDatabases_KindergartenId",
                table: "FileToDatabases");
        }
    }
}
