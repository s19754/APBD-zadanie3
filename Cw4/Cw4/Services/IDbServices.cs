using Cw4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Services
{
    public interface IDbServices 
    {
        IEnumerable<Animal> GetAnimals(string orderBy);
        string[] AddAnimal(Animal animal);
        string[] EditAnimal(Animal animal, int idAnimal);
        string[] DeleteAnimal(int idAnimal);
    }
}
