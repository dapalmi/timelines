using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Timelines.Domain;
using Timelines.Domain.Person;

namespace Timelines.ViewModels
{
    public class RelationshipViewModel
    {
        public int PersonId { get; set; }
        public PersonViewModel Person { get; set; }

        public int RelatedPersonId { get; set; }
        public PersonViewModel RelatedPerson { get; set; }

        public string RelationshipType { get; set; }
    }
}
