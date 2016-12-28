using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timelines.Migrations
{
    public partial class LongLatDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Place",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Place",
                type: "decimal(9,6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Place",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Place",
                nullable: true);
        }
    }
}
