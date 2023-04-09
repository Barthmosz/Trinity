using Trinity.Application.DTOs.Product;

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
