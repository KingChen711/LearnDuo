using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorDemand.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCretedUpdatedDateForTeachingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "TeachingSchedules",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "TeachingSchedules",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "TeachingSchedules");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "TeachingSchedules");
        }
    }
}
