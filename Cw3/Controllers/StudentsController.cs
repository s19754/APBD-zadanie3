using Cw3.Models;
using Cw3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw3.Controllers
{
    
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IFileDbService _fileDbService;
        public StudentsController(IFileDbService fileDbService)
        {
            _fileDbService = fileDbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _fileDbService.GetStudents();
            return Ok(students);
        }

        [HttpGet("{indexNumber}")]
        //[HttpGet]
        //[Route("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            var student = _fileDbService.GetStudent(indexNumber);
            if (student is null) return NotFound("Nie ma takiego studenta");
            return Ok(student);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
             var students = _fileDbService.GetStudents();
            if (!students.Contains(student))
            {
                if (string.IsNullOrEmpty(student.FirstName) ||
                    string.IsNullOrEmpty(student.LastName) ||
                    string.IsNullOrEmpty(student.Mail) ||
                    string.IsNullOrEmpty(student.MothersName) ||
                    string.IsNullOrEmpty(student.FathersName) ||
                    string.IsNullOrEmpty(student.IndexNumber) ||
                    string.IsNullOrEmpty(student.Studies) ||
                    string.IsNullOrEmpty(student.Mode) ||
                    student.BirthDate.Equals(null))
                {
                    return BadRequest($"Za mało danych");
                }
                else
                {
                    _fileDbService.AddStudent(student);
                    return Created("", student);
                }
            }
            else
            {
                return BadRequest($"Już istnieje student z indexem {student.IndexNumber}");
            }
        }

        [HttpPut("{indexNumber}")]
        public IActionResult PutStudent(string indexNumber, Student student)
        {
            try
            {
                _fileDbService.PutStudent(indexNumber, student);
                return Created("", student);
            }
            catch (ArgumentException e)
            {
                return BadRequest($"{e}");
            }
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            var students = _fileDbService.GetStudents();
            Regex regex = new Regex(@"s\d+");
            if (!regex.IsMatch(indexNumber))
            {
                return BadRequest($"nieprawidłowy index");
            }
            if (_fileDbService.GetStudent(indexNumber).Equals(null))
            {
                return NotFound($"Nie ma takiego studenta");
            }
            _fileDbService.DeleteStudent(indexNumber);
            return Ok($"Usunięto studenta {indexNumber}");
        }

    }
}
