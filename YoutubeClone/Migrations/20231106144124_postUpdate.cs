using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YoutubeClone.Migrations
{
    /// <inheritdoc />
    public partial class postUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PostModels",
                newName: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostModels",
                newName: "Id");
        }
    }
}
