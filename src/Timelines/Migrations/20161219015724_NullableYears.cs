using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timelines.Migrations
{
    public partial class NullableYears : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UnknownStart",
                table: "Persons",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnknownEnd",
                table: "Persons",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Start",
                table: "Persons",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "End",
                table: "Persons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UnknownStart",
                table: "Persons",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "UnknownEnd",
                table: "Persons",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Start",
                table: "Persons",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "End",
                table: "Persons",
                nullable: false);
        }
    }
}
