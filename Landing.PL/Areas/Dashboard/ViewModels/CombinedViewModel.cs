namespace Landing.PL.Areas.Dashboard.ViewModels
{
    public class CombinedViewModel
    {
        public List<BlogFormVM> Blogs { get; set; }
        public List<ServiceFormVM> Services { get; set; }
        public List<ShopFormVM> Shops { get; set; }
        public List<TeamFormVM> Teams { get; set; }
        public List<TestimonialFormVM> Testimonials { get; set; }
    }
}
