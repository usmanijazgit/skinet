using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } 
        public List<BasketItemDto> ItemS { get; set; }
    }
    
}