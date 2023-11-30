using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _logger = logger;
        }

        // Implementation of IUserService methods
        public async Task<IList<Person>> SearchPerson(string searchTerm)
        {
            return await _personRepository.SearchPerson(searchTerm);
        }
        
    }
}
