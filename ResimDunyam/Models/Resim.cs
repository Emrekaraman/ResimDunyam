using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResimDunyam.Models
{
    public class Resim
    {
        public int Id { get; set; }

        [Required]
        public string ResimYolu { get; set; }

        [Required]
        public DateTime? YuklenmeZamani { get; set; }

        public string IdentityUserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
