using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Timelines.Persistence;

namespace Timelines.Migrations
{
    [DbContext(typeof(TimelinesContext))]
    [Migration("20160822184913_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Timelines.Domain.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<string>("Text");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Timelines.Domain.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("End");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Meaning");

                    b.Property<string>("Name");

                    b.Property<int>("Start");

                    b.Property<int>("UnkownEnd");

                    b.Property<int>("UnkownStart");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Timelines.Domain.PersonEvent", b =>
                {
                    b.Property<int>("PersonId");

                    b.Property<int>("EventId");

                    b.HasKey("PersonId", "EventId");

                    b.HasIndex("EventId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonEvent");
                });

            modelBuilder.Entity("Timelines.Domain.Relationship", b =>
                {
                    b.Property<int>("PersonId");

                    b.Property<int>("RelatedPersonId");

                    b.Property<int>("RelationshipType");

                    b.HasKey("PersonId", "RelatedPersonId");

                    b.HasIndex("PersonId");

                    b.HasIndex("RelatedPersonId");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("Timelines.Domain.PersonEvent", b =>
                {
                    b.HasOne("Timelines.Domain.Event", "Event")
                        .WithMany("PersonEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Timelines.Domain.Person", "Person")
                        .WithMany("PersonEvents")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Timelines.Domain.Relationship", b =>
                {
                    b.HasOne("Timelines.Domain.Person", "Person")
                        .WithMany("PersonRelationships")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Timelines.Domain.Person", "RelatedPerson")
                        .WithMany("RelatedPersonRelationships")
                        .HasForeignKey("RelatedPersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
