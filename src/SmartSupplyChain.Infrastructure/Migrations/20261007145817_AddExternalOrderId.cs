using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupplyChain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalOrderId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalOrderId",
                table: "Orders");
        }
    }
}
