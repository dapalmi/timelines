using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Timelines.Domain.Person;

namespace Timelines.Migrations
{
    public partial class AddGenderTypeForPersons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderType",
                table: "Persons",
                nullable: false,
                defaultValue: GenderType.Unknown);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderType",
                table: "Persons");
        }
    }
}
