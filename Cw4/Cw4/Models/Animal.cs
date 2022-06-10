using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Models
{
    public class Animal
    {
        public int IdAnimal { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
    }
}
