using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoltCargo.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class RelationShipMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectTime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisConnectTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "HasRequestPending",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelationShip",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisConnectTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HasRequestPending",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsRelationShip",
                table: "AspNetUsers");
        }
    }
}
