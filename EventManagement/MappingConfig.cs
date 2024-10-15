using EventManagement.Data.Models;
using AutoMapper;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Models.ModelsDto.AgendaDtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<Organization, OrganizationCreateDto>().ReverseMap();
            CreateMap<Organization, OrganizationUpdateDto>().ReverseMap();

            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Event, EventCreateDto>().ReverseMap();
            CreateMap<EventUpdateDto, Event>()
                .ForMember(dest => dest.OrganizationId, opt => opt.Ignore());

            CreateMap<Agenda, AgendaDto>().ReverseMap();
            CreateMap<Agenda, AgendaCreateDto>().ReverseMap();
            CreateMap<AgendaUpdateDto, Agenda>().ReverseMap();
        }
    }
}