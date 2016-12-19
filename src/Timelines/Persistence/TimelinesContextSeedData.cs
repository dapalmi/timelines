using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timelines.Domain.Event;
using Timelines.Domain.Person;
using Timelines.Domain.Relationship;

namespace Timelines.Persistence
{
    public class TimelinesContextSeedData
    {
        private readonly TimelinesContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TimelinesContextSeedData(TimelinesContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            _context.Database.Migrate();

            if (await _userManager.FindByEmailAsync("dapalmi@t-online.de") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Matthias",
                    Email = "dapalmi@t-online.de"
                };
                await _userManager.CreateAsync(user, "P@ssw0rd!");
            }

            if (!_context.Persons.Any())
            {
                var adam = new Person()
                {
                    Name = "Adam",
                    Meaning = "Earthling Man, Mankind",
                    Start = -4026,
                    End = -3970,
                    UnknownStart = 0,
                    UnknownEnd = 0,
                    ImageUrl = "http://i.imgur.com/00n9NWT.jpg",
                };

                var adamBirth = new Event()
                {
                    Name = "Birth",
                    Year = -4026,
                    Text = "#Birth"
                };

                var adamBirthEvent = new PersonEvent()
                {
                    Person = adam,
                    Event = adamBirth
                };

                adam.PersonEvents.Add(adamBirthEvent);

                _context.Persons.Add(adam);
                _context.Events.Add(adamBirth);

                var eve = new Person()
                {
                    Name = "Eve",
                    Meaning = "Living One",
                    Start = -4000,
                    End = -3970,
                    UnknownStart = -4026,
                    UnknownEnd = -3965,
                    ImageUrl = "http://i.imgur.com/00n9NWT.jpg",
                };

                var eveBirth = new Event()
                {
                    Name = "Birth",
                    Year = -4026,
                    Text = "#Birth"
                };

                var eveBirthEvent = new PersonEvent()
                {
                    Person = eve,
                    Event = eveBirth
                };

                eve.PersonEvents.Add(eveBirthEvent);

                _context.Persons.Add(eve);
                _context.Events.Add(eveBirth);

                var relationshipAdamEve = new Relationship()
                {
                    Person = adam,
                    RelatedPerson = eve,
                    RelationshipType = RelationshipType.Spouse
                };

                var relationshipEveAdam = new Relationship()
                {
                    Person = eve,
                    RelatedPerson = adam,
                    RelationshipType = RelationshipType.Spouse
                };

                _context.Relationships.Add(relationshipAdamEve);
                _context.Relationships.Add(relationshipEveAdam);

                await _context.SaveChangesAsync();
            }
        }
    }
}
