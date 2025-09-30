using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutputManagementComponent.Migrations
{
    /// <inheritdoc />
    public partial class contactmethode : Migration
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
