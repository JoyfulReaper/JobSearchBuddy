using AutoMapper;
using JobSearchBuddy.Server.Jobs.Interfaces;
using JobSearchBuddy.Server.Notes;
using JobSearchBuddy.Shared.Jobs;
using JobSearchBuddy.Shared.Notes;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchBuddy.Server.Jobs;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IJobRepository _jobRepository;

    public JobsController(
        IMapper mapper,
        IJobRepository jobRepository)
    {
        _mapper = mapper;
        _jobRepository = jobRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobReadDto>>> GetJobs()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<JobReadDto>>(jobs));
    }

    [HttpGet("{jobId}")]
    public async Task<ActionResult<JobReadDto>> GetJob(int jobId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);

        if (job == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<JobReadDto>(job));
    }

    [HttpPost]
    public async Task<ActionResult<JobReadDto>> CreateJob(JobCreateDto jobCreateDto)
    {
        var job = _mapper.Map<Job>(jobCreateDto);

        await _jobRepository.CreateAsync(job);

        return CreatedAtAction(nameof(GetJob), new { jobId = job.JobId }, _mapper.Map<JobReadDto>(job));
    }

    [HttpPut("{jobId}")]
    public async Task<IActionResult> UpdateJob(int jobId, JobCreateDto jobUpdateDto)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);

        if (job == null)
        {
            return NotFound();
        }

        _mapper.Map(jobUpdateDto, job);

        await _jobRepository.UpdateAsync(job);

        return NoContent();
    }

    [HttpDelete("{jobId}")]
    public async Task<IActionResult> DeleteJob(int jobId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);

        if (job == null)
        {
            return NotFound();
        }

        await _jobRepository.DeleteAsync(job.JobId);
        return NoContent();
    }

    [HttpPost("notes/{jobId}")]
    public async Task<ActionResult<NoteReadDto>> AddNote(int jobId, NoteCreateDto noteCreateDto)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null)
        {
            return NotFound($"Job with id {jobId} not found.");
        }

        var note = _mapper.Map<Note>(noteCreateDto);
        note.RelationshipType = "Job"; // TODO: Make an enum
        await _jobRepository.AddNoteAsync(jobId, note);

        return NoContent();
    }

    [HttpDelete("notes/{jobId}/{noteId}")]
    public async Task<ActionResult<NoteReadDto>> DeleteNote(int jobId, int noteId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null)
        {
            return NotFound($"Job with id {jobId} not found.");
        }

        await _jobRepository.DeleteNoteAsync(jobId, noteId);

        return NoContent();
    }
}