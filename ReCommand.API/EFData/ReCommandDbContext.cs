using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReCommand.API.EFData.Entities;

namespace ReCommand.API.EFData
{
    public class ReCommandDbContext : DbContext
    {
        public DbSet<ProjectReCommand> ProjectReCommands { get; set; }
        public DbSet<ProjectReferenceUser> ProjectReferenceUsers { get; set; }

        public ReCommandDbContext(DbContextOptions<ReCommandDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectReCommand>().ToTable("ProjectReCommands")
                .HasKey(x => x.Id);

            modelBuilder.Entity<ProjectReferenceUser>().ToTable("ProjectReferenceUsers")
                .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
