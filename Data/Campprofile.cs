using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class Campprofile : Profile
    {
        public Campprofile()
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, l => l.MapFrom(e => e.Location.VenueName))
                .ForMember(c => c.PostalCode, l => l.MapFrom(e => e.Location.PostalCode))
                .ReverseMap();


            this.CreateMap<Talk, TalkModel>()
                .ForMember(t => t.Camp, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
