using AutoMapper;
using Landing.DAL.Models;
using Landing.PL.Areas.Dashboard.ViewModels;

namespace Landing.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<ServiceFormVM, Service>().ReverseMap();
            CreateMap<Service, ServiceVM>().ReverseMap();
            CreateMap<Service, ServiceDetailsVM>().ReverseMap(); 
            
            
            CreateMap<ShopFormVM, Shop>().ReverseMap();
            CreateMap<Shop, ShopVM>().ReverseMap();
            CreateMap<Shop, ShopDetailsVM>().ReverseMap();
            
            
            CreateMap<TestimonialFormVM, Testimonial>().ReverseMap();
            CreateMap<Testimonial, TestimonialVM>().ReverseMap();
            CreateMap<Testimonial, TestimonialDetailsVM>().ReverseMap();

            
            CreateMap<TeamFormVM, Team>().ReverseMap();
            CreateMap<Team, TeamVM>().ReverseMap();
            CreateMap<Team, TeamDetailsVM>().ReverseMap(); 
            
            CreateMap<BlogFormVM, Blog>().ReverseMap();
            CreateMap<Blog, BlogVM>().ReverseMap();
            CreateMap<Blog, BlogDetailsVM>().ReverseMap();
        }
    }
}
