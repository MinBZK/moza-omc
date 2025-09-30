using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OutputManagementComponent.Migrations
{
    /// <inheritdoc />
    public partial class EventList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "notificaties",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_notificaties_Reference",
                table: "notificaties",
                column: "Reference");

            migrationBuilder.CreateTable(
                name: "contactmethodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Attempted = table.Column<bool>(type: "boolean", nullable: false),
                    NotificatieEntityDbId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactmethodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contactmethodes_notificaties_NotificatieEntityDbId",
                        column: x => x.NotificatieEntityDbId,
                        principalTable: "notificaties",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificatieevents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Event = table.Column<string>(type: "text", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateLastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificatieevents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notificatieevents_notificaties_Reference",
                        column: x => x.Reference,
                        principalTable: "notificaties",
                        principalColumn: "Reference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notificaties_Reference",
                table: "notificaties",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contactmethodes_NotificatieEntityDbId",
                table: "contactmethodes",
                column: "NotificatieEntityDbId");

            migrationBuilder.CreateIndex(
                name: "IX_notificatieevents_Reference",
                table: "notificatieevents",
                column: "Reference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contactmethodes");

            migrationBuilder.DropTable(
                name: "notificatieevents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_notificaties_Reference",
                table: "notificaties");

            migrationBuilder.DropIndex(
                name: "IX_notificaties_Reference",
                table: "notificaties");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "notificaties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
