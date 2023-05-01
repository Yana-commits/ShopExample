using FruitShop.Host.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FruitShop.Host.Data.Entities
{
    public class FruitItemEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("FruitTypeId")]
        public FruitTypeEntity FruitType { get; set; }
        [Display(Name ="Fruit Type")]
        public int FruitTypeId { get; set; }
        [ForeignKey("FruitSortId")]
        public FruitSortEntity FruitSort { get; set; }
        [Display(Name = "Fruit Sort")]
        public int FruitSortId { get; set; }

        public string Description { get; set; } = null!;
        [Range(1,int.MaxValue)]
        public decimal Price { get; set; }
        [ForeignKey("ProviderId")]
        public ProviderEntity Provider { get; set; }
        public int ProviderId { get; set; }
        public string PictureUrl { get; set; } = null!;
    }
}
