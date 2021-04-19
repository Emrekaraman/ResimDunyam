using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResimDunyam.ViewModels
{
    public class YeniResimViewModel
    {
        [Required]
        [Display(Name = "Resim")]
        public IFormFile Resim { get; set; }
    }
}
