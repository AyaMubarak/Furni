﻿namespace Landing.PL.Areas.Dashboard.ViewModels
{
    public class BlogFormVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy
        {
            get; set;
        }
      

    }
}
