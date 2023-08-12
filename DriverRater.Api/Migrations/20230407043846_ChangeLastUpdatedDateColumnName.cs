using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverRater.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLastUpdatedDateColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRankedUtc",
                table: "Drivers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateRanked",
                table: "Drivers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRanked",
                table: "Drivers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRankedUtc",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
