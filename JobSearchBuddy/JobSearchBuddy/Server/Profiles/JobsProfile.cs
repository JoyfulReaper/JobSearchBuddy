using AutoMapper;
using JobSearchBuddy.Server.Jobs;
using JobSearchBuddy.Shared.Jobs;

namespace JobSearchBuddy.Server.Profiles;

public class JobsProfile : Profile
{
    public JobsProfile()
    {
        CreateMap<Job, JobReadDto>();
        CreateMap<JobCreateDto, Job>();
    }
}