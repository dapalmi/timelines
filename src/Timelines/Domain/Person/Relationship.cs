using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timelines.Domain.Person
{
    public class Relationship
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int RelatedPersonId { get; set; }
        public Person RelatedPerson { get; set; }

        public RelationshipType RelationshipType { get; set; }
    }
}
