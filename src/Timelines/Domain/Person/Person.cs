using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Timelines.Domain.Person
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public string Meaning { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int? UnknownStart { get; set; }
        public int? UnknownEnd { get; set; }
        public string ImageUrl { get; set; }

        public List<PersonEvent> PersonEvents { get; set; } = new List<PersonEvent>();

        [InverseProperty("Person")]
        public List<Relationship.Relationship> PersonRelationships { get; set; }

        [InverseProperty("RelatedPerson")]
        public List<Relationship.Relationship> RelatedPersonRelationships { get; set; }
    }
}
