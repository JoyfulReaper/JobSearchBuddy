using AutoMapper;
using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Shared.Contacts;

namespace JobSearchBuddy.Server.Profiles;

public class ContactsProfile : Profile
{
    public ContactsProfile()
    {
        CreateMap<Contact, ContactReadDTO>();
        CreateMap<ContactCreateDto, Contact>();
    }
}