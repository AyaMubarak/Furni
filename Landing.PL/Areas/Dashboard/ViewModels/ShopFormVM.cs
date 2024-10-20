using Microsoft.Identity.Client;

namespace Landing.PL.Areas.Dashboard.ViewModels
{
    public class ShopFormVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Price { get; set; }

    }
}
