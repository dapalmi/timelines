using System.Collections.Generic;
using Timelines.Domain;
using Timelines.Domain.Person;

namespace Timelines.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int? PreviousEventYear { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
    }
}
