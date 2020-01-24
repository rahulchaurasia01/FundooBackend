using FundooCommonLayer.ModelDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooRepositoryLayer.ModelContext
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { set; get; }

        public DbSet<NotesDetails> NotesDetails { set; get; }

        public DbSet<LabelDetails> LabelDetails { set; get; }

        public DbSet<NotesLabel> NotesLabels { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetails>()
                .HasIndex(user => user.EmailId)
                .IsUnique();

        }
    }
}
