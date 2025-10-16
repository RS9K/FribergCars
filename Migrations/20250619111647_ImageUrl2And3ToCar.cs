using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCars.Migrations
{
    /// <inheritdoc />
    public partial class ImageUrl2And3ToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl2",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl3",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl2",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ImageUrl3",
                table: "Cars");
        }
    }
}
