using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResimDunyam.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResimDunyam.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resim> Resimler { get; set; }
    }
}
