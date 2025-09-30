using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutputManagementComponent.Migrations
{
    /// <inheritdoc />
    public partial class StatusBijEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "notificatieevents",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "notificatieevents");
        }
    }
}
