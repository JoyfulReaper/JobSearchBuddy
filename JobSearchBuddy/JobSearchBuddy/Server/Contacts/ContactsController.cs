using AutoMapper;
using JobSearchBuddy.Server.Contacts.Interfaces;
using JobSearchBuddy.Shared.Contacts;
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
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        var contacts = await _contactRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ContactReadDTO>>(contacts));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ContactReadDTO>(contact));
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(ContactCreateDto contactCreateDto)
    {
        var contact = _mapper.Map<Contact>(contactCreateDto);

        await _contactRepository.CreateAsync(contact);

        return CreatedAtAction(nameof(GetContact), new { id = contact.ContactId }, _mapper.Map<ContactReadDTO>(contact));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, ContactCreateDto contactUpdateDto)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        _mapper.Map(contactUpdateDto, contact);

        await _contactRepository.UpdateAsync(contact);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        await _contactRepository.DeleteAsync(contact.ContactId);

        return NoContent();
    }
}