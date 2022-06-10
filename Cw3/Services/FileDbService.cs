using Cw3.Exceptions;
using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public class FileDbService : IFileDbService
    {
        public static List<Student> Students = new List<Student>();
        public static HashSet<string> Indexes = new HashSet<string>();

        public static bool IsParsed = false;
        static FileDbService()
        {
            
        }

        public static async Task CsvParse()
        {
            Students = new List<Student>();
            string[] result2 = await File.ReadAllLinesAsync(Directory.GetCurrentDirectory() + @"\Data\students.csv");
            foreach (var item in result2)
            {
                string[] kolumny = item.Split(',');
                if (kolumny.Length == 9)
                {
                    bool goodFormat = true;
                    foreach (string str in kolumny)
                    {
                        if (string.IsNullOrEmpty(str))
                        {
                            goodFormat = false;
                        }
                    }
                    if (goodFormat)
                    {
                        var usCulture = "en-US";
                        DateTime Birthdate = DateTime.Parse(kolumny[3], new CultureInfo(usCulture, false));
                        Student tempStudent = new Student(
                            kolumny[0], // First Name
                            kolumny[1], // Last Name
                            kolumny[2], // IndexNumber
                            Birthdate,
                            kolumny[4], // Studies
                            mode: kolumny[5], // Mode
                            kolumny[6], // Mail
                            kolumny[8], // FathersName
                            kolumny[7]); //MothersName
                        if (Students.Equals(null))
                        {
                            FileDbService.Students.Add(tempStudent);
                            Indexes.Add(tempStudent.IndexNumber);
                        }
                        else
                        {
                            if (Indexes.Contains(tempStudent.IndexNumber))
                            {
                                throw new DataFormatException($"Drugie wystąpienie użytkownika {tempStudent.IndexNumber} nie zostało zapisane");
                            }
                            else
                            {
                               Students.Add(tempStudent);
                            }
                        }
                    }
                    else
                    {
                        throw new DataFormatException("Błąd rekordów, conajmniej jedna kolumna jest pusta");
                    }
                }
                else
                {
                    throw new DataFormatException("Błąd rekordów, za mało danych (kolumn nie jest 9)");
                }
            }
            IsParsed = true;
        }



        public void AddStudent(Student student)
            
        {
            if (!IsParsed)_ = CsvParse();
            if (Indexes.Contains(student.IndexNumber))
            {
                throw new ArgumentException();
            }
            else
            {
                Students.Add(student);
                _ = CsvAdd(student);
            }
        }

        public Student GetStudent(string indexNumber)
        {
            if (!IsParsed) _ = CsvParse();
            return Students.Find(e => e.IndexNumber == indexNumber);
        }

        public IEnumerable<Student> GetStudents()
        {
            if (!IsParsed)_ = CsvParse();
            return Students;
        }

        public void PutStudent(string indexNumber, Student student)
        {
            if (!IsParsed) _ = CsvParse();
            if (indexNumber != student.IndexNumber)
                throw new ArgumentException("Index ścieżki i studenta są niezgodne");
            int index = Students.FindIndex(e => e.IndexNumber == student.IndexNumber);
            if (index != -1)
            {
                Students[index] = student;
                _ = CsvSaveAsync();
            }
            else
            {
                throw new ArgumentException("Nie ma studenta z takim indeksem");
            }
        }

        public void DeleteStudent(string indexNumber)
        {
            if (!IsParsed) _ = CsvParse();
            Students.Remove(Students.Find(e => e.IndexNumber == indexNumber));
            _ = CsvSaveAsync();
        }

        public async static Task CsvSaveAsync()
        {
            try
            {
                using StreamWriter sr = new StreamWriter(@"C:\Users\Tomek\Desktop\Cw3\Data\students.csv");
                foreach (var item in Students)
                {
                    await sr.WriteAsync($"{item.FirstName},{item.LastName},{item.IndexNumber},{item.BirthDate.ToString("MM")}/{item.BirthDate.ToString("dd")}/{item.BirthDate.ToString("yyyy")},{item.Studies},{item.Mode},{item.Mail},{item.FathersName},{item.MothersName}\n");
                }
            }catch (IOException e)  
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        public async static Task CsvAdd(Student student)
        {
            try
            {
                using StreamWriter sr = File.AppendText(@"C:\Users\Tomek\Desktop\Cw3\Data\students.csv");
                await sr.WriteAsync($"{student.FirstName},{student.LastName},{student.IndexNumber},{student.BirthDate.ToString("MM")}/{student.BirthDate.ToString("dd")}/{student.BirthDate.ToString("yyyy")},{student.Studies},{student.Mode},{student.Mail},{student.FathersName},{student.MothersName}\n");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }


}
