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
using EventManagement.Models.ModelsDto.PurchasedDtos;
using EventManagement.Models.SupportChatRoomDtos;
using EventManagement.Data.Models.ChatRoom;

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

            CreateMap<MemberOrganization, MemberOrganizationDto>().ReverseMap();
            CreateMap<ApplicationUser, UserOrganizationDto>().ReverseMap();
            CreateMap<PurchasedTicket, PurchasedTicketDto>().ReverseMap();
            CreateMap<SupportChatRoom, SupportChatRoomDto>().ReverseMap();
            CreateMap<MessageDto, Message>();
            CreateMap<Message, MessageDto>().ForMember(dest => dest.SendAt, opt =>
                    opt.MapFrom(src => src.SendAt.ToString("yyyy-MM-dd HH:mm:ss")));

            //Query
            CreateMap<ModelApi.EventDetailViewDto, ModelData.EventDetailViewDto>().ReverseMap();
            CreateMap< ModelData.TicketTimeViewDto, ModelApi.TicketTimeViewDto>().ForMember(dest => dest.ScheduledDate, opt =>
                    opt.MapFrom(src => src.ScheduledDate.ToString("yyyy-MM-dd")));

            CreateMap<PurchasedTicketDto, PurchasedTicket>().ReverseMap();
            CreateMap<PurchasedTicketUpdateDto, PurchasedTicket>().ReverseMap();

        }
    }
}