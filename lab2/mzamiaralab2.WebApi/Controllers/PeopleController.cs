
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mzamiaralab2.WebApi.Context;
using mzamiaralab2.WebApi.Models;

namespace mzamiaralab2.WebApi.Controllers
{
  
    [ApiController]
    [Route("api/people")]
    public class PeopleController : ControllerBase
    {
        AzureDbContext _azureDbContext;
        public PeopleController(AzureDbContext azureDbContext)
        {
            _azureDbContext = azureDbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_azureDbContext.People);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var person = _azureDbContext.People.FirstOrDefault(p => p.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(person);
            }
        }

        [HttpPost] 
        public IActionResult Create([FromBody] Person person)
        {
            _azureDbContext.People.Add(person);
            _azureDbContext.SaveChanges();
            return Ok(person.PersonId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var person = _azureDbContext.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }
            _azureDbContext.Remove(person);
            _azureDbContext.SaveChanges();
            return Ok("Usunięto");
        }

        [HttpPut]
        public IActionResult Put([FromRoute] Person person)
        {
            _azureDbContext.Update(person);
            _azureDbContext.SaveChanges();
            return Ok(person.PersonId);
        }
    }
}