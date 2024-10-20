using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Landing.PL.Data.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Shops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
