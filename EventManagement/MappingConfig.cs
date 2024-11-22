using EventManagement.Data.Models;
using AutoMapper;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Models.ModelsDto.AgendaDtos;
using EventManagement.Models.ModelsDto.EventDateDtos;
using EventManagement.Models.ModelsDto.TicketDtos;
using ModelApi =  EventManagement.Models.ModelQueries;
using ModelData = EventManagement.Data.Queries.ModelDto;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using EventManagement.Models.ModelsDto.OrderDetailDtos;
using EventManagement.Data.Queries.ModelDto;
using static EventManagement.Models.ModelsDto.OrganizationDtos.MemberOrganizationDto;
using EventManagement.Models.ModelsDto.PurchasedDtos;

namespace EventManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Repository
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

            CreateMap<EventDate, EventDateDto>()
                .ForMember(dest => dest.ScheduledDate, opt =>
                    opt.MapFrom(src => src.ScheduledDate.ToString("yyyy-MM-dd")));
            CreateMap<EventDateSaveDto, EventDate>();

            CreateMap<TicketCreateDto, Ticket>().ReverseMap();
            CreateMap<TicketDto, Ticket>().ReverseMap();
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.SaleStartDate, opt =>
                    opt.MapFrom(src => src.SaleStartDate.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.SaleEndDate, opt =>
                    opt.MapFrom(src => src.SaleEndDate.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Ticket, TicketUpdateDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderCreateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDto>().ReverseMap();

            CreateMap<OrderHeader, OrderHeaderDto>().ForMember(dest => dest.OrderDate, opt =>
                    opt.MapFrom(src => src.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<DataOverviewOrderDto, OverviewOrderDto>().ForMember(dest => dest.OrderDate, opt =>
                    opt.MapFrom(src => src.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<MemberOrganization, MemberOrganizationDto>().ReverseMap();
            CreateMap<ApplicationUser, UserOrganizationDto>().ReverseMap();
            //Query
            CreateMap<ModelApi.SearchItemDto.HomeEventDto, ModelData.HomeEventDto>().ReverseMap();
            CreateMap<ModelApi.EventDetailViewDto, ModelData.EventDetailViewDto>().ReverseMap();
            CreateMap< ModelData.TicketTimeViewDto, ModelApi.TicketTimeViewDto>().ForMember(dest => dest.ScheduledDate, opt =>
                    opt.MapFrom(src => src.ScheduledDate.ToString("yyyy-MM-dd")));

            CreateMap<PurchasedTicketDto, PurchasedTicket>().ReverseMap();
            CreateMap<PurchasedTicketUpdateDto, PurchasedTicket>().ReverseMap();

        }
    }
}