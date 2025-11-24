namespace GlobalAutoMarketplaceFrontend.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Bname { get; set; } = null!;
    }

    public class BrandCreateDto
    {
        public string Bname { get; set; } = null!;
    }
}
