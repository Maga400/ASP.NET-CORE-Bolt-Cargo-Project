using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoltCargo.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class _2Finish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClientFinish",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDriverFinish",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClientFinish",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDriverFinish",
                table: "Orders");
        }
    }
}
