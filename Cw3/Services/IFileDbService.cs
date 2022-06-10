using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public interface IFileDbService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudent(string indexNumber);
        void AddStudent(Student student);
        void PutStudent(string indexNumber, Student student);
        void DeleteStudent(string indexNumber);
    }
}
