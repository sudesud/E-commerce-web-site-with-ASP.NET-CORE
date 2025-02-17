namespace Bitik.Models
{
    public class FavoriteItem
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }

        public FavoriteItem() { }

        public FavoriteItem(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Image = product.ImageUrl;
        }
    }
}
