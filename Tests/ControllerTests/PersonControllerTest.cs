using Core.Interfaces;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Test.API.Controllers;

namespace ControllerTests
{
    public class PersonControllerTests
    {
        private readonly Mock<ILogger<PersonController>> _loggerMock;
        private readonly Mock<IPersonService> _personServiceMock;

        public PersonControllerTests()
        {
            _loggerMock = new Mock<ILogger<PersonController>>();
            _personServiceMock = new Mock<IPersonService>();
        }

        [Fact]
        public async Task GetPerson_ShouldReturn_1Object()
        {
            // Arrange
            var person = new List<Person>
            {
                new Person {Id = 1, FirstName = "Antony", LastName = "Fitt", Email = "afitt0@a8.net", Gender = "Male" }
            };

            _personServiceMock.Setup(p => p.SearchPerson("Antony")).ReturnsAsync(person);

            var controller = new PersonController(_loggerMock.Object, _personServiceMock.Object);

            // Act
            var result = controller.Get("Antony");

            // Assert
            var okResponse = Assert.IsType<OkObjectResult>(result);
            var personList = okResponse.Value as List<Person>;
            Assert.Single(personList);
            Assert.Equal(person, personList);
        }

        [Fact]
        public async Task GetPersonNowt_ShouldReturn_BadRequest()
        {
            // Arrange

            var person = new List<Person>
            {
                new Person {Id = 1, FirstName = "Antony", LastName = "Fitt", Email = "afitt0@a8.net", Gender = "Male" }
            };

            _personServiceMock.Setup(p => p.SearchPerson("Antony")).ReturnsAsync(person);

            var controller = new PersonController(_loggerMock.Object, _personServiceMock.Object);

            // Act
            var result = controller.Get(string.Empty);

            // Assert
            var badResponse = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(badResponse.Value);
            
            Assert.Equal(Core.Constants.Messages.SearchTermMandatory, problemDetails.Title);

        }

        [Fact]
        public async Task GetPersonLargeSearchTerm_ShouldReturn_BadRequest()
        {
            // Arrange

            var person = new List<Person>
            {
                new Person {Id = 1, FirstName = "Antony", LastName = "Fitt", Email = "afitt0@a8.net", Gender = "Male" }
            };

            _personServiceMock.Setup(p => p.SearchPerson("Antony")).ReturnsAsync(person);

            var controller = new PersonController(_loggerMock.Object, _personServiceMock.Object);

            // Act
            var result = controller.Get("sdasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasd");

            // Assert
            var badResponse = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(badResponse.Value);

            Assert.Equal(Core.Constants.Messages.SearchTermTooLong, problemDetails.Title);

        }
    }
}
