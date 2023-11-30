using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Text.Json;

namespace Core.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        // Implementation of IRepository methods
        private IEnumerable<Person> _people;
        private readonly ILogger<PersonRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonRepository(ILogger<PersonRepository> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }

        private void GetPersonData(string path)
        {
            string jsonContent = File.ReadAllText(path);
            if (jsonContent == null)
            {
                _logger.LogCritical("No Person JSON data found");
            }
            _people = JsonSerializer.Deserialize<IEnumerable<Person>>(jsonContent);
        }

        public async Task<IList<Person>> SearchPerson(string searchTerm)
        {
            var jsonPath = _configuration["jsonFilePath"];
            if (jsonPath == null)
            {
                _logger.LogCritical(Constants.Messages.MissingConfiguration);
                return new List<Person>();
            }
            GetPersonData(jsonPath);

            string[] searchTerms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Person>();

            //First pass, check explicitly for a 2 term firstname/lastname match
            if (searchTerms.Length == 2)
            {
                result.AddRange(_people.Where(p =>
                    (p.FirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (p.LastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0)));
            }

            //Then do the generic match if no items are found
            if ( result.Count == 0) { 
                foreach (var term in searchTerms)
                {
                    result.AddRange(_people.Where(p =>
                        (p.FirstName?.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (p.LastName?.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (p.Email?.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)));
                }

                // Use Distinct to remove any duplicates
                if (result.Count > 1)
                {
                    result = result.Distinct(new PersonEqualityComparer()).ToList();
                }
            }

            return result;
        }

        public class PersonEqualityComparer : IEqualityComparer<Person>
        {
            public bool Equals(Person x, Person y)
            {
                return x?.Id == y?.Id;
            }
            public int GetHashCode(Person obj)
            {
                return obj?.Id.GetHashCode() ?? 0;
            }
        }
    }
}
