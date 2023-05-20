using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BitirmeProjesi.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace BitirmeProjesi.Data
{
    public partial class BitirmeDBContext : IdentityDbContext<User, Role, int>
    {
        public BitirmeDBContext()
        {
        }

        public BitirmeDBContext(DbContextOptions<BitirmeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Period> Periods { get; set; } = null!;
        public virtual DbSet<PeriodStudent> PeriodStudents { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

             optionsBuilder.UseSqlServer("Data Source=(localdb)\\IEEE;Initial Catalog=Bitirme;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

                                         


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



    } 
}
