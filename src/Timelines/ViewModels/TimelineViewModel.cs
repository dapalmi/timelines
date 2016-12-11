using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Timelines.Domain;
using Timelines.Domain.Person;

namespace Timelines.ViewModels
{
    public class TimelineViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Meaning { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int UnkownStart { get; set; }
        public int UnkownEnd { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<EventViewModel> Events { get; set; }
        public IEnumerable<int> Parents { get; set; }
        public IEnumerable<int> Children { get; set; }
        public IEnumerable<int> Spouse { get; set; }
    }
}
