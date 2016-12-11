﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timelines.Domain;
using Timelines.Domain.Event;
using Timelines.Domain.Person;

namespace Timelines.Persistence
{
    public class TimelinesContext : IdentityDbContext<IdentityUser>
    {
        public TimelinesContext(DbContextOptions<TimelinesContext> options) 
            : base(options)
        { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEvent>()
                .HasKey(t => new { t.PersonId, t.EventId });

            modelBuilder.Entity<PersonEvent>()
                .HasOne(pe => pe.Person)
                .WithMany(p => p.PersonEvents)
                .HasForeignKey(pe => pe.PersonId);

            modelBuilder.Entity<PersonEvent>()
                .HasOne(pe => pe.Event)
                .WithMany(e => e.PersonEvents)
                .HasForeignKey(pe => pe.EventId);

            modelBuilder.Entity<Relationship>()
                .HasKey(t => new {t.PersonId, t.RelatedPersonId});

            base.OnModelCreating(modelBuilder);
        }
    }
}
