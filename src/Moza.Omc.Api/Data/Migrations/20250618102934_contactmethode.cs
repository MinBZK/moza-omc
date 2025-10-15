using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moza.Omc.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContactMethode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "contactmethodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "contactmethodes");
        }
    }
}
