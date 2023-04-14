using AutoMapper;
using JobSearchBuddy.Server.Notes;
using JobSearchBuddy.Shared.Notes;

namespace JobSearchBuddy.Server.Profiles;

public class NotesProfile : Profile
{
    public NotesProfile()
    {
        CreateMap<Note, NoteReadDto>();
        CreateMap<NoteCreateDto, Note>();
    }
}