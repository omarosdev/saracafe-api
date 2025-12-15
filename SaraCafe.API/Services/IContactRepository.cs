using SaraCafe.API.Entities;

namespace SaraCafe.API.Services;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact> CreateContactAsync(Contact contact);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> SaveChangesAsync();
}

