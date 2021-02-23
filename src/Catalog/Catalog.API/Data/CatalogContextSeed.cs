using System;
using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void Seed(IMongoCollection<Product> productCollection)
        {
            var isProductExist = productCollection.Find(c => true).Any();
            if (!isProductExist)
            {
                productCollection.InsertManyAsync(GetPreConfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Xiaomi Mi 9",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "Lorem ipsum",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    Category = "Smartphone"
                },
                new Product
                {
                    Name = "iPhone X2",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "Lorem ipsum",
                    ImageFile = "product-5.png",
                    Price = 680.00M,
                    Category = "Smartphone"
                },
                new Product
                {
                    Name = "iPhone X2 Pro",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "Lorem ipsum",
                    ImageFile = "product-5.png",
                    Price = 880.00M,
                    Category = "Smartphone"
                }
            };
        }
    }
}
