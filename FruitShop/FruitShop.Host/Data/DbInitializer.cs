using FruitShop.Host.Data.Entities;

namespace FruitShop.Host.Data
{
    public static class DbInitializer
    {

        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.FruitSortEntities.Any())
            {
                await context.FruitSortEntities.AddRangeAsync(GetPreconfiguredFruitSorts());

                await context.SaveChangesAsync();
            }

            if (!context.FruitTypeEntities.Any())
            {
                await context.FruitTypeEntities.AddRangeAsync(GetPreconfiguredFruitTypes());

                await context.SaveChangesAsync();
            }

            if (!context.ProviderEntities.Any())
            {
                await context.ProviderEntities.AddRangeAsync(GetPreconfiguredProviders());

                await context.SaveChangesAsync();
            }

            if (!context.FruitItemEntities.Any())
            {
                await context.FruitItemEntities.AddRangeAsync(GetPreconfiguredFruitItems());

                await context.SaveChangesAsync();
            }
        }
        private static IEnumerable<FruitSortEntity> GetPreconfiguredFruitSorts()
        {
            return new List<FruitSortEntity>()
            {
              new FruitSortEntity() { Sort = "Makintosh" },
              new FruitSortEntity() { Sort = "SnowCalvill" },
              new FruitSortEntity() { Sort = "Gold" },
              new FruitSortEntity() { Sort = "Isabella" },
              new FruitSortEntity() { Sort = "No grade" },
            };
        }
        private static IEnumerable<FruitTypeEntity> GetPreconfiguredFruitTypes()
        {
            return new List<FruitTypeEntity>()
            {
              new FruitTypeEntity() { Type = "Fruit" },
              new FruitTypeEntity() { Type = "Berry" },
              new FruitTypeEntity() { Type = "Nut" },
              new FruitTypeEntity() { Type = "Dried Fruit" },
              new FruitTypeEntity() { Type = "Else" },

            };
        }
        private static IEnumerable<ProviderEntity> GetPreconfiguredProviders()
        {
            return new List<ProviderEntity>()
            {
              new ProviderEntity() { Name = "Provider", Address = "Address" },
               new ProviderEntity() { Name = "Provider2", Address = "Address" },
               new ProviderEntity() { Name = "Provider3", Address = "Address" },
               new ProviderEntity() { Name = "Provider4", Address = "Address" },
            };
        }
        private static IEnumerable<FruitItemEntity> GetPreconfiguredFruitItems()
        {
            return new List<FruitItemEntity>()
            {
              new FruitItemEntity() { Name ="Apple",FruitTypeId = 1,FruitSortId = 1,Description = "good one",Price = 20,ProviderId =1,PictureUrl = "apple.png"},
              new FruitItemEntity() { Name ="Pinapple",FruitTypeId = 1,FruitSortId = 3,Description = "good one",Price = 200,ProviderId =2,PictureUrl = "ananas.png"},
              new FruitItemEntity() { Name ="Strawberry",FruitTypeId = 2,FruitSortId = 5,Description = "good one",Price = 50,ProviderId =3,PictureUrl = "strawberrie.png"},
              new FruitItemEntity() { Name ="Banana",FruitTypeId = 1,FruitSortId = 5,Description = "good one",Price = 40,ProviderId =1,PictureUrl = "banan.png"},
              new FruitItemEntity() { Name ="Garpe",FruitTypeId = 2,FruitSortId = 4,Description = "good one",Price = 50,ProviderId =2,PictureUrl = "grapes.png"},
               new FruitItemEntity() { Name ="Apple",FruitTypeId = 1,FruitSortId = 2,Description = "good one",Price = 20,ProviderId =1,PictureUrl = "calvill.png"},
               new FruitItemEntity() { Name ="Cherry",FruitTypeId = 2,FruitSortId = 5,Description = "good one",Price = 50,ProviderId =1,PictureUrl = "cherry.png"},
               new FruitItemEntity() { Name ="Hazelnut",FruitTypeId = 3,FruitSortId = 5,Description = "good one",Price = 50,ProviderId =3,PictureUrl = "nut.png"},
            };
        }
    }
}
