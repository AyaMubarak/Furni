namespace Landing.PL.Areas.Dashboard.ViewModels
{
    public class TeamFormVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
      
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
