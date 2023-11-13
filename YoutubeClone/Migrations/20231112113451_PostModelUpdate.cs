using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YoutubeClone.Migrations
{
    /// <inheritdoc />
    public partial class PostModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "PostModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "PostModels");
        }
    }
}
