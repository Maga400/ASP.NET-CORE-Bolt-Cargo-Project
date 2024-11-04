using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoltCargo.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class FinishMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinish",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinish",
                table: "Orders");
        }
    }
}
