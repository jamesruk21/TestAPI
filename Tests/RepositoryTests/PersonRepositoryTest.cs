using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;

namespace RepositoryTests
{
    public class PersonRepositoryTest
    {

        private readonly Mock<ILogger<PersonRepository>> _loggerMock;
        private readonly Mock<IConfiguration> _configuration;

        public PersonRepositoryTest()
        {
            _loggerMock = new Mock<ILogger<PersonRepository>>();
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["jsonFilePath"]).Returns("./peopleTestData.json");
        }

        [Fact]
        public async Task SearchPersonForJames_ShouldReturn_2PersonObjects()
        {
            // Arrange
            var searchTerm = "James";
            var personRepository = new PersonRepository(_loggerMock.Object, _configuration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Collection(result,
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Equal(searchTerm, person.FirstName);
                },
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Equal(searchTerm, person.FirstName);
                }
            );
        }

        [Fact]
        public async Task SearchPersonForjam_ShouldReturn_3PersonObjects()
        {
            // Arrange
            var searchTerm = "jam";
            var personRepository = new PersonRepository(_loggerMock.Object, _configuration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Collection(result,
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Contains(searchTerm, person.FirstName.ToLower());
                },
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Contains(searchTerm, person.FirstName.ToLower());
                },
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Contains(searchTerm, person.Email.ToLower());
                }
            );
        }

        [Fact]
        public async Task SearchPersonForKateySoltan_ShouldReturn_3PersonObjects()
        {
            // Arrange
            var firstName = "Katey";
            var lastName = "Soltan";
            var searchTerm = String.Concat("Katey", " ", "Soltan");
            var personRepository = new PersonRepository(_loggerMock.Object, _configuration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Single(result); 
            Assert.Collection(result,
                person =>
                {
                    Assert.NotNull(person);
                    Assert.Equal(firstName, person.FirstName);
                    Assert.Equal(lastName, person.LastName);
                }
            );
        }

        [Fact]
        public async Task SearchPersonForJasmineDuncan_ShouldReturn_0PersonObjects()
        {
            // Arrange
            var searchTerm = "Jasmine Duncan";
            var personRepository = new PersonRepository(_loggerMock.Object, _configuration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchPersonForEmpty_ShouldReturn_0PersonObjects()
        {
            // Arrange
            var searchTerm = "";
            var personRepository = new PersonRepository(_loggerMock.Object, _configuration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchPersonEmptyConfig_ShouldReturn_0PersonObjects()
        {
            // Arrange
            var localConfiguration = new Mock<IConfiguration>();
            var searchTerm = "James";
            var personRepository = new PersonRepository(_loggerMock.Object, localConfiguration.Object); // Pass the mock as a dependency

            // Act
            var result = await personRepository.SearchPerson(searchTerm);

            // Assert
            Assert.NotNull(result);

            Assert.Empty(result);

            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(), // LogLevel
                    It.IsAny<EventId>(),  // EventId
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(Core.Constants.Messages.MissingConfiguration)), // Message
                    It.IsAny<Exception>(), // Exception
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>() // Formatter
                ),
                Times.Once
        );
        }
    }

}