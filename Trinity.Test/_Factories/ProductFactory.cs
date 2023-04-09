using Trinity.Application.DTOs.Product;
using Trinity.Domain.Entities;

namespace Trinity.Test._Factories
{
    public static class ProductFactory
    {
        public static ProductAddInput MakeProductAddInput()
        {
            return new ProductAddInput()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1
            };
        }

        public static ProductUpdateInput MakeProductUpdateInput()
        {
            return new ProductUpdateInput()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 1
            };
        }

        public static ProductOutput MakeProductOutput()
        {
            return new ProductOutput()
            {
                Id = "any_id",
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 1
            };
        }

        public static Product MakeProduct()
        {
            return new Product()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 1
            };
        }
    }
}
