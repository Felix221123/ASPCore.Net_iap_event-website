using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soft20181_starter.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendeeCountToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendeeCount",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendeeCount",
                table: "Events");
        }
    }
}
