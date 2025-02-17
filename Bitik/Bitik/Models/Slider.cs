using System;
using System.ComponentModel.DataAnnotations;

namespace Bitik.Models
{
    public class Slider
    {
        public int SliderId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string RedirectUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
