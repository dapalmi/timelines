using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timelines.Migrations
{
    public partial class StartAndEndYearRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Start",
                table: "Persons",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "End",
                table: "Persons",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Start",
                table: "Persons",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "End",
                table: "Persons",
                nullable: true);
        }
    }
}
