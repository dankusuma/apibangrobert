using Login.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Services.Models
{
    public class UserContext : DbContext
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {


        }


        public DbSet<UserClass> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserClass>().ToTable("User");

        }


    }
}
