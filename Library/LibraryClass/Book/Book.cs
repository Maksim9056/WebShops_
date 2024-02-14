using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LibraryClass.Book
{
    /// <summary>
    /// Class for Book
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }

        public string Name { get; set; }
        public string Year_of_publication { get; set; }
        public Book(int id, string author, string name, string year_of_publication)
        {
            Id = id;
            Author = author;
            Name = name;
            Year_of_publication = year_of_publication;
        }

        public Book()
        {
        }
    }
}
