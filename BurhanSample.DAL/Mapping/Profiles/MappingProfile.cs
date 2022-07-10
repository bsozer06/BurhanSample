using AutoMapper;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DataAccess.Mapping.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.FullAddress, opt =>
                opt.MapFrom(src => string.Join(' ', src.Address, src.Country)));
        }
    }
}
