using EventManagement.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using EventManagement.Models.ModelsDto;

namespace EventManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Organization, OrganizationResponse>().ReverseMap();
        }
    }
}