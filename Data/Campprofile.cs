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
            this.CreateMap<Camp, CampModel>().ForMember(c=>c.Venue, l=>l.MapFrom(e => e.Location.VenueName))
                .ForMember(c => c.Address,
                    l => l.MapFrom(e => e.Location.Address1)).ForMember(c => c.City,
                    l => l.MapFrom(e => e.Location.CityTown)).ForMember(c => c.State,
                    l => l.MapFrom(e => e.Location.StateProvince)).ForMember(c => c.PostalCode,
                    l => l.MapFrom(e => e.Location.PostalCode)).ForMember(c => c.Country,
                    l => l.MapFrom(e => e.Location.Country));
        }
    }
}
