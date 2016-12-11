using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Timelines.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    End = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Meaning = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Start = table.Column<int>(nullable: false),
                    UnkownEnd = table.Column<int>(nullable: false),
                    UnkownStart = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonEvent",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEvent", x => new { x.PersonId, x.EventId });
                    table.ForeignKey(
                        name: "FK_PersonEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonEvent_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false),
                    RelatedPersonId = table.Column<int>(nullable: false),
                    RelationshipType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.PersonId, x.RelatedPersonId });
                    table.ForeignKey(
                        name: "FK_Relationships_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Relationships_Persons_RelatedPersonId",
                        column: x => x.RelatedPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonEvent_EventId",
                table: "PersonEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonEvent_PersonId",
                table: "PersonEvent",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_PersonId",
                table: "Relationships",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_RelatedPersonId",
                table: "Relationships",
                column: "RelatedPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonEvent");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
