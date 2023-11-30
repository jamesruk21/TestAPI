using Core.Interfaces;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ServiceTests
{
    public class PersonServiceTest
    {
        private readonly Mock<ILogger<PersonService>> _loggerMock;
        private readonly Mock<IPersonRepository> _personRepository;

        public PersonServiceTest()
        {
            _loggerMock = new Mock<ILogger<PersonService>>();
            _personRepository = new Mock<IPersonRepository>();
        }

        [Fact]
        public async Task SearchPersonForJames_ShouldReturn_2PersonObjects()
        {
            // Arrange
            var person = new List<Person>
            {
                new Person {Id = 1, FirstName = "Antony", LastName = "Fitt", Email = "afitt0@a8.net", Gender = "Male" }
            };

            _personRepository.Setup(p => p.SearchPerson(It.IsAny<string>())).ReturnsAsync(person);
            var personService = new PersonService(_personRepository.Object, _loggerMock.Object); // Pass the mock as a dependency

            // Act
            var result = await personService.SearchPerson(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);

            Assert.Equal(result, person);
        }

    }

}