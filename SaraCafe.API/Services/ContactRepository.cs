using Microsoft.EntityFrameworkCore;
using SaraCafe.API.DbContexts;
using SaraCafe.API.Entities;

namespace SaraCafe.API.Services;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _context.Contacts
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await SaveChangesAsync();
        return contact;
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var contact = await GetContactByIdAsync(id);
        if (contact == null)
        {
            return false;
        }

        contact.IsRead = true;
        return await SaveChangesAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}

