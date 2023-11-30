using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Test.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [HttpGet()]
        public IActionResult Get([FromQuery] string searchTerm )
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest(new ProblemDetails { Title = Core.Constants.Messages.SearchTermMandatory });
            }

            if (searchTerm.Length > 50)
            {
                return BadRequest(new ProblemDetails { Title = Core.Constants.Messages.SearchTermTooLong });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(_personService.SearchPerson(searchTerm).Result);
            }

        }
    }
}