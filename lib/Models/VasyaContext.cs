using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
 
namespace Telegram.Bot.vasya_v2
{
    public partial class VasyaContext : DbContext
    {
        public DbSet<vasya_v2.TDialog> Dialogs { get; set; }
        public DbSet<vasya_v2.THomeTask> HomeTasks { get; set; }
        public DbSet<vasya_v2.TSubject> Subjects { get; set; }
        public DbSet<vasya_v2.TTimeTable> TimeTables { get; set; }
         
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