using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Models
{
    public class Student
    {
        public Student( string firstName, string lastName, string indexNumber, DateTime birthDate, string studies, string mode, string mail, string mothersName, string fathersName)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Studies = studies;
            Mode = mode;
            Mail = mail;
            MothersName = mothersName;
            FathersName = fathersName;
        }

        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Studies { get; set; }
        public string Mode { get; set; }
        public string Mail { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   IndexNumber == student.IndexNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndexNumber);
        }
    }

    



}
