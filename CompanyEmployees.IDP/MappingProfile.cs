using AutoMapper;
using CompanyEmployees.IDP.Entities;
using CompanyEmployees.IDP.Pages.Create;

namespace CompanyEmployees.IDP
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InputModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
