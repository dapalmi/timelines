using System.ComponentModel.DataAnnotations.Schema;

namespace Timelines.Domain.Relationship
{
    public class Relationship
    {
        public int PersonId { get; set; }
        public Person.Person Person { get; set; }

        public int RelatedPersonId { get; set; }
        public Person.Person RelatedPerson { get; set; }

        public RelationshipType RelationshipType { get; set; }
    }
}
