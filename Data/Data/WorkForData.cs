using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.LibraryClass.Book;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class WorkForData:DbContext
    {
        public WorkForData(DbContextOptions<WorkForData> options) : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = null!;

        public WorkForData()
        {
            try
            {


                Database.EnsureCreated();
            }
            catch
            {

            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");

        }
    }
}
