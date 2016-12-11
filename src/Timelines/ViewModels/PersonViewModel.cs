using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Timelines.Domain;
using Timelines.Domain.Person;

namespace Timelines.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Meaning { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int UnkownStart { get; set; }
        public int UnkownEnd { get; set; }
        public string ImageUrl { get; set; }
    }
}
