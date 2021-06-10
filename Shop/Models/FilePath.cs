using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class FilePath
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Path { get; set; }
        [Required]
        public int ProductId { get; set; }

        public FilePath(String path)
        {
            Path = path;       
        }

    
    }
}
