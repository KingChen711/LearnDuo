using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorDemand.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaidPriceInTeachingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidPrice",
                table: "TeachingSchedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaidPrice",
                table: "TeachingSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
