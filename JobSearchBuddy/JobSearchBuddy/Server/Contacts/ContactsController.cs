using AutoMapper;
using JobSearchBuddy.Server.Contacts.Interfaces;
using JobSearchBuddy.Server.Notes;
using JobSearchBuddy.Shared.Contacts;
using JobSearchBuddy.Shared.Notes;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchBuddy.Server.Contacts;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IContactRepository _contactRepository;

    public ContactsController(
        IMapper mapper,
        IContactRepository contactRepository)
    {
        _mapper = mapper;
        _contactRepository = contactRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetContacts()
    {
        var contacts = await _contactRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ContactReadDto>>(contacts));
    }

    [HttpGet("{contactId}")]
    public async Task<ActionResult<ContactReadDto>> GetContact(int contactId)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);

        if (contact == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ContactReadDto>(contact));
    }

    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> CreateContact(ContactCreateDto contactCreateDto)
    {
        var contact = _mapper.Map<Contact>(contactCreateDto);

        await _contactRepository.CreateAsync(contact);

        return CreatedAtAction(nameof(GetContact), new { contactId = contact.ContactId }, _mapper.Map<ContactReadDto>(contact));
    }

    [HttpPut("{contactId}")]
    public async Task<IActionResult> UpdateContact(int contactId, ContactCreateDto contactUpdateDto)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);

        if (contact == null)
        {
            return NotFound();
        }

        _mapper.Map(contactUpdateDto, contact);

        await _contactRepository.UpdateAsync(contact);

        return NoContent();
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> DeleteContact(int contactId)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);

        if (contact == null)
        {
            return NotFound();
        }

        await _contactRepository.DeleteAsync(contact.ContactId);
        return NoContent();
    }

    [HttpPost("notes/{contactId}")]
    public async Task<ActionResult<NoteReadDto>> AddNote(int contactId, NoteCreateDto noteCreateDto)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);
        if (contact == null)
        {
            return NotFound($"Contact with id {contactId} not found.");
        }

        var note = _mapper.Map<Note>(noteCreateDto);
        note.RelationshipType = "Contact"; // TODO: Make an enum
        await _contactRepository.AddNoteAsync(contactId, note);

        return NoContent();
    }

    [HttpDelete("notes/{contactId}/{noteId}")]
    public async Task<ActionResult<NoteReadDto>> DeleteNote(int contactId, int noteId)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);
        if (contact == null)
        {
            return NotFound($"Contact with id {contactId} not found.");
        }

        await _contactRepository.DeleteNoteAsync(contactId, noteId);

        return NoContent();
    }
}
