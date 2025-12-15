using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaraCafe.API.DTOs;
using SaraCafe.API.Entities;
using SaraCafe.API.Services;

namespace SaraCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;

    public ContactsController(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    // POST: api/contacts
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto createContactDto)
    {
        var contact = new Contact
        {
            Name = createContactDto.Name,
            Email = createContactDto.Email,
            Phone = createContactDto.Phone,
            Message = createContactDto.Message
        };

        var createdContact = await _contactRepository.CreateContactAsync(contact);

        var contactDto = new ContactDto
        {
            Id = createdContact.Id,
            Name = createdContact.Name,
            Email = createdContact.Email,
            Phone = createdContact.Phone,
            Message = createdContact.Message,
            CreatedAt = createdContact.CreatedAt,
            IsRead = createdContact.IsRead
        };

        return CreatedAtAction(nameof(GetContact), new { id = contactDto.Id }, contactDto);
    }

    // GET: api/contacts
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
    {
        var contacts = await _contactRepository.GetContactsAsync();
        var contactDtos = contacts.Select(c => new ContactDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Message = c.Message,
            CreatedAt = c.CreatedAt,
            IsRead = c.IsRead
        });

        return Ok(contactDtos);
    }

    // GET: api/contacts/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ContactDto>> GetContact(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        
        if (contact == null)
        {
            return NotFound();
        }

        // Mark as read when viewed
        await _contactRepository.MarkAsReadAsync(id);

        var contactDto = new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Phone = contact.Phone,
            Message = contact.Message,
            CreatedAt = contact.CreatedAt,
            IsRead = true // Will be marked as read
        };

        return Ok(contactDto);
    }
}

