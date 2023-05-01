using System.ComponentModel.DataAnnotations;

namespace FruitShop.Host.Data.Entities
{
    public class FruitSortEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sort { get; set; }
    }
}
