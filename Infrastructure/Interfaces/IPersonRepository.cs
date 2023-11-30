using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Interfaces
{
    public interface IPersonRepository
    {
        Task<IList<Person>> SearchPerson(string searchTerm);

    }
}
