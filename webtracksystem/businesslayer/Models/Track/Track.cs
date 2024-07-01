using System;
using Repositories.Dto;

namespace BusinessLayer.Models
{
    public class Track
    {
        public int Id { get; set; }
        public long UniqCreatedTicks { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedUtcDate { get; set; }
    }
}