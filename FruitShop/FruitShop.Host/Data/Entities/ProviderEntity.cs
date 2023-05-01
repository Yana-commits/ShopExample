using System.ComponentModel.DataAnnotations;

namespace FruitShop.Host.Data.Entities
{
    public class ProviderEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Address { get; set; }
    }
}
