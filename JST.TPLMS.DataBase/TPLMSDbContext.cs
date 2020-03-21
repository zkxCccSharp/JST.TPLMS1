using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using JST.TPLMS.Entitys;

namespace JST.TPLMS.DataBase
{
    public class TPLMSDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        //public virtual DbSet<Role> Role { get; set; }

        //public virtual DbSet<Organization> Organization { get; set; }

        public TPLMSDbContext(DbContextOptions<TPLMSDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //省略
        }
    }
}
