using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timelines.Migrations
{
    public partial class NameAdjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnkownEnd",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "UnkownStart",
                table: "Persons");

            migrationBuilder.AddColumn<int>(
                name: "UnknownEnd",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnknownStart",
                table: "Persons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnknownEnd",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "UnknownStart",
                table: "Persons");

            migrationBuilder.AddColumn<int>(
                name: "UnkownEnd",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnkownStart",
                table: "Persons",
                nullable: false,
                defaultValue: 0);
        }
    }
}
