using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Cw4.Models;
using Cw4.Services;

namespace Cw4.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {

        private readonly IDbServices _dbService;
        public AnimalController(IDbServices dbServices)
        {
            _dbService = dbServices;
        }

        [HttpGet]
        public IActionResult GetAnimals(String orderBy = "name")
        {
            var res = _dbService.GetAnimals(orderBy);

            if (orderBy.ToLower() != "name" &&
                orderBy.ToLower() != "description" &&
                orderBy.ToLower() != "category" &&
                orderBy.ToLower() != "area")
            {
                return BadRequest("Wrong orderBy value select from: name, description, category or area");
            }
            else
            {
                return Ok(res);
            }
        }

        [HttpPost]
        public IActionResult AddAnimal([FromBody] Models.Animal animal)
        {

            if (IsAnyNullOrEmpty(animal))
            {
                return BadRequest("Missing one or more parameter in the JSON");
            }
            else
            {

                string[] res = _dbService.AddAnimal(animal);
                if (res[0] == "Ok")
                {
                    return Created("", res[1]);
                }
                else if (res[0] == "Error")
                {
                    return BadRequest(res[1]);
                }
                else
                {
                    return BadRequest("Unknown error");

                }

            }
        }

        [HttpPut("{idAnimal}")]
        public IActionResult EditAnimal([FromBody] Models.Animal animal,
                                              [FromRoute] int idAnimal)
        {
            if (IsAnyNullOrEmpty(animal))
            {
                return BadRequest("Missing one or more parameter in the JSON");
            }
                else
                {
                    string[] res = _dbService.EditAnimal(animal, 
                        idAnimal);
                    if (res[0] == "Ok")
                    {
                        return Ok(res[1]);
                    }
                    else if (res[0] == "Error")
                    {
                        return BadRequest(res[1]);
                    }
                    else
                    {
                        return BadRequest("Unknown error");

                    }
                }
        }

        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal([FromRoute] int idAnimal)
        {


            string[] res = _dbService.DeleteAnimal(idAnimal);
            if (res[0] == "Ok")
            {
                return Ok(res[1]);
            }
            else if (res[0] == "Error")
            {
                return BadRequest(res[1]);
            }
            else if (res[0] == "File Not Found")
            {
                return NotFound(res[1]);
            }
            else
            {
                return BadRequest("Unknown error");

            }
        }
       

        bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo propertyInfo in myObject.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    string value = (string)propertyInfo.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
    
