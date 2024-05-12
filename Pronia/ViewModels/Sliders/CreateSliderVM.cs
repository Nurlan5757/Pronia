using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.Sliders
{
    public class CreateSliderVM
    {
        [MaxLength(32, ErrorMessage = "32den cox olmasin"),Required]
        public string Title { get; set; }
        [Range(0, 100), Required]
        public int Discount { get; set; }
        [MaxLength(64), Required]
        public string Subtitle { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}

