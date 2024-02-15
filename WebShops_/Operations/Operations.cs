using Data.Data;
using Library.LibraryClass.Book;

namespace WebShops_.Operations
{
    public class Operations
    {

        public void Start()
        {
            try
            {
                using (WorkForData data = new WorkForData())
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void Add()
        {
            var ListBook = CreateBook();

            try
            {
                using (WorkForData data = new WorkForData())
                {
                    //var users = db.Users.Where(p => p.Name_Employee == regis_Users.Name_Employee);

                    //Count_roles = users;

                    //IQueryable<Book> books =  ;
                    Book book = new Book();
                    for (int i = 0; i < ListBook.Count; i++)
                    {
                        book = data.Book.FirstOrDefault(u => u.Name == ListBook[i].Name);
                    }
                    //foreach (Book user in books)
                    //{

                    //}

                    if (book == null)
                    {

                        for (int i = 0; i < ListBook.Count; i++)
                        {
                            data.Book.AddRange(ListBook[i]);
                            data.SaveChanges();
                        }

                    }
                    else
                    {

                    }

                }
            }
            catch (Exception)
            {
                using (WorkForData data = new WorkForData())
                {
                    for (int i = 0; i < ListBook.Count; i++)
                    {
                        data.Book.AddRange(ListBook[i]);
                        data.SaveChanges();
                    }
                }
            }
        }
        private List<Book> CreateBook()
        {
            List<Book> list = new List<Book>();

            try
            {


                Book book = new Book(0, "Мартин", "ВЫСОКО-НАГРУЖЕННЫЕ ПРИЛОЖЕНИЯ", "2022");
                Book book1 = new Book(0, "Countre", ".Selenium.WebDriver.Recipes in C#", "2024");
                Book book2 = new Book(0, "Jonathan", "ASP.NET 8 Best Practices", "2023");
                Book book3 = new Book(0, "Nabendu Biswas", "Apress.Practical.GraphQL.", "2023");
                Book book4 = new Book(0, "Zubair Chowhan", "Net.Framework.100.professional.notes.", "2023");
                Book book5 = new Book(0, "Roger Ye", "Packt.NET.MAUI.Cross - Platform.", "2023");
                Book book6 = new Book(0, "Симан Марк", "Vnedrenie_zavisimostey_na_platforme_NET", "2021");
                Book book7 = new Book(0, "Jason Alls", "Clean Code in C#", "2020");
                Book book8 = new Book(0, "James Charlesworth", "Developing on  AWS with C#", "2023");
                Book book9 = new Book(0, "Valerio De Sanctis", "Building Web APIs with ASP.NET Core", "2023");

                list.Add(book);
                list.Add(book1);
                list.Add(book2);
                list.Add(book3);
                list.Add(book4);
                list.Add(book5);
                list.Add(book6);
                list.Add(book7);
                list.Add(book8);
                list.Add(book9);
            }
            catch
            {

            }
            return list;
        }
        public List<Book> SelectBook()
        {
            List<Book> books = new List<Book>();

            try
            {
                using (WorkForData data = new WorkForData())
                {
                    books = data.Book.ToList();
                }
                return books;
            }
            catch (Exception)
            {

            }
            return books;
        }
        public Book SelectBookFirstId(int id)
        {
            Book book = null;
            try
            {

                using (WorkForData data = new WorkForData())
                {
                    book = data.Book.FirstOrDefault(u => u.Id == id);
                }
                return book;
            }
            catch (Exception)
            {

            }
            return book;
        }
        public Book DeletyBookFirstId(int id)
        {
            Book book = null;
            try
            {

                using (WorkForData data = new WorkForData())
                {
                    book = data.Book.FirstOrDefault(u => u.Id == id);

                     data.Book.Remove(book);
                    data.SaveChanges();

                }
                return book;
            }
            catch (Exception)
            {

            }
            return book;
        }
    }
}
