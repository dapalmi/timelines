using System.Collections.Generic;
using Timelines.Domain;
using Timelines.Domain.Person;

namespace Timelines.ViewModels
{
    public class PlaceViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}
