using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
 
namespace Telegram.Bot.vasya_v2
{
    public partial class VasyaContext : DbContext
    {
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<HomeTask> HomeTasks { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
         
        public VasyaContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}