using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M223PunchclockDotnet.Migrations
{
    /// <inheritdoc />
    public partial class EntryDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Entry",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Entry");
        }
    }
}
