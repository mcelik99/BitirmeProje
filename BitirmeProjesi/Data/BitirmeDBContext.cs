using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BitirmeProjesi.Models;

namespace BitirmeProjesi.Data
{
    public partial class BitirmeDBContext : IdentityDbContext<User, Role, int>
    {
        public BitirmeDBContext(DbContextOptions<BitirmeDBContext> options) : base(options)
        {

        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Period> Periods { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<ParticipantTeacher> ParticipantTeachers { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    } 
}
