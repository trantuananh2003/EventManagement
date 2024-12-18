﻿using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries.ModelDto
{
    public class EventDetailViewDto
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string UrlImage { get; set; }
        public string Location { get; set; }
        public string Coordinates { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
        public string NameOrganization { get; set; }
        public string UrlImageOrganization { get; set; }
        public string Status { get; set; }
        public string Privacy { get; set; }
        public string EventType { get; set; }
        public List<TicketTimeViewDto> Tickets { get; set; }
        public List<EventDateViewDto> EventDates {get;set;}
    }

    public class TicketTimeViewDto
    {
        public string NameTicket { get; set; }  //from Model Ticket
        public DateTime ScheduledDate { get; set; } //from Model EventDate
    }

    public class EventDateViewDto
    {
        public string DateTitle { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
