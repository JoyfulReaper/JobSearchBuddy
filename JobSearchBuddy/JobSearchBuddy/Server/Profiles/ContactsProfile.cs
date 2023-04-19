using AutoMapper;
using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Shared.Contacts;

namespace JobSearchBuddy.Server.Profiles;

public class ContactsProfile : Profile
{
    public ContactsProfile()
    {
        CreateMap<Contact, ContactReadDto>()
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        CreateMap<ContactCreateDto, Contact>();
    }
}