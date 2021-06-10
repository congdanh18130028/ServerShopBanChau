using Microsoft.EntityFrameworkCore;
using Shop.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(255)")]
        public String Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(25)")]
        public String Phone { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public String Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public String Role { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public String Email { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public String Password { get; set; }
    }
}
