using FruitShop.Host.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FruitShop.Host.Data.Entities
{
    public class FruitTypeEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
