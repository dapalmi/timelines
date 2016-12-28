using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timelines.Migrations
{
    public partial class LongLatNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Place",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Place",
                nullable: false);
        }
    }
}
