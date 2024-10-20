namespace Landing.PL.Areas.Dashboard.ViewModels
{
    public class ServiceDetailsVM
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt{ get; set; }
        public bool ISDeleted {  get; set; }
        public string ImageName {  get; set; }

    }
}
