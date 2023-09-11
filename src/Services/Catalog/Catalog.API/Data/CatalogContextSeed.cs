using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Catalog.API.Data {
  internal class CatalogContextSeed {
    public static void SeedData(IMongoCollection<Product> products) {
      var existProduct = products.Find(p => true).Any();
      if (existProduct == false) {
        products.InsertManyAsync(GetPreconfiguredProducts());
      }
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() {
      return new List<Product> {
        new Product {
          Name = "IPhone X",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-1.png",
          Price = 950.00M,
          Category = "Smart Phone"
        },
        new Product {
          Name = "Samsung 10",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-2.png",
          Price = 840.00M,
          Category = "Smart Phone"
        },
        new Product {
          Name = "Huawei Plus",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-3.png",
          Price = 650.00M,
          Category = "White Appliances"
        },
        new Product {
          Name = "Xiaomi Mi 9",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-4.png",
          Price = 470.00M,
          Category = "White Appliances"
        },
        new Product {
          Name = "Samsung A70",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-5.png",
          Price = 360.00M,
          Category = "Smart Phone"
        },
        new Product {
          Name = "LG ThinQ",
          Summary = "Summary",
          Description = "Description",
          ImageFile = "product-6.png",
          Price = 240.00M,
          Category = "White Appliances"
        }
      };
    }
  }
}
