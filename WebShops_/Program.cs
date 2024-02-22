using Microsoft.EntityFrameworkCore;
using System;
using Library.LibraryClass.Book;
using Data.Data;
using System.Configuration;
using Microsoft.Extensions.Options;
namespace WebShops_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers(); // Добавление сервисов для контроллеров API          
            builder.Services.AddDbContext<WorkForData>(options =>
             options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            Operations.Operations work = new Operations.Operations();
            work.Start();

            work.Add();
            bool d = false;
            app.UseStaticFiles(); // Добавляем middleware для обслуживания статических файлов из папки wwwroot


            app.MapWhen(context => context.Request.Path.StartsWithSegments("/"), appBuilder =>
            {
                appBuilder.Run(async (context) =>
                {
                    var stringBuilder = new System.Text.StringBuilder();
                    bool isPostRequest = context.Request.Method == "POST";
                    var indexHtmlContent = System.IO.File.ReadAllText("wwwroot/html/index.html");

                    // Паттерн для поиска placeholder в таблице
                    var pattern = "<!-- Вставляем данные о книгах с помощью C# кода -->";

                    // Создаем массив строк, разделяя содержимое файла по placeholder
                    var parts = indexHtmlContent.Split(new[] { pattern }, StringSplitOptions.None);
                    if (!isPostRequest)
                    {
                        stringBuilder.Append($"{parts[0]}");

                        // Отображение списка всех книг

                        var allBooks = work.SelectBook();
                        foreach (var book in allBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a>><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }

                        // Заменяем placeholder на данные о книгах в HTML-коде index.html
                        stringBuilder.Append($"{parts[1]}");
                        // Обновленный HTML-код с данными о книгах
                    }
                    else
                    {
                        var form = await context.Request.ReadFormAsync();
                        var author = form["author"];
                        var name = form["name"];
                        var date = form["date"];
                        var id = form["id"];

                        // Применяем фильтры к вашим данным (например, списку книг)
                        var filteredBooks = work.SelectBook().Where(book =>
                            (string.IsNullOrEmpty(author) || book.Author.Contains(author)) &&
                            (string.IsNullOrEmpty(name) || book.Name.Contains(name)) &&
                            (string.IsNullOrEmpty(date) || book.Year_of_publication.ToString().Contains(date)) &&
                            (string.IsNullOrEmpty(id) || book.Id.ToString().Contains(id))
                        );
                        stringBuilder.Append($"{parts[0]}");

                        // Отображаем результаты фильтрации
                        foreach (var book in filteredBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }
                        stringBuilder.Append($"{parts[1]}");

                    }

                    context.Response.ContentType = "text/html; charset=utf-8"; // Установка правильной кодировки
                    await context.Response.WriteAsync(stringBuilder.ToString());
                });
            });
            // Обработка запроса, если URL начинается с "/book/"
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/book"), appBuilder =>
            {
                appBuilder.Run(async context =>
                  {
                      // HTML-форма для ввода данных о книге

                  string id = "";
                  var path = context.Response.HttpContext.Request.Path.Value;
                  string pattern = @"/book/(\d+)";

                  System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(path, pattern);
                  if (match.Success)
                  {
                      id = match.Groups[1].Value;
                  }
                  var data = work.SelectBookFirstId(Convert.ToInt32(id));

                  context.Response.StatusCode = 200;
                      // Формируем HTML-форму
                      var formHtml = $@"<style>/* Стили для формы */ form {{
                background-color: #f9f9f9;
                border: 1px solid #ddd;
                padding: 10px;
                border-radius: 5px;
                margin-bottom: 10px;
                width: 300px; /* Ширина формы */
            }}
            label {{
                display: block;
                margin-bottom: 5px;
            }}
            input[type='text'] {{
                width: 100%;
                padding: 5px;
                margin-bottom: 10px;
                border: 1px solid #ccc;
                border-radius: 3px;
            }}
            button[type='submit'] {{
                background-color: #4CAF50;
                color: white;
                padding: 10px 15px;
                border: none;
                border-radius: 4px;
                cursor: pointer;
                width: 100%;
            }}
            button[type='submit']:hover {{
                background-color: #45a049;
            }}
        </style>
        <form method='post'>
            <div>
                <label for='bookName'>{data.Name}:</label>
            </div>
            <div>
                <label for='authorName'>{data.Author}:</label>
            </div>
            <div>
                <label for='publicationYear'>{data.Year_of_publication}:</label>
            </div>
            <button type='submit' name='buyButton'>Купить</button> <!-- Поменяли type на submit и добавили name для кнопки -->
            <input type='hidden' name='bookId' value='{id}'> <!-- Передаем ID книги как скрытое поле -->
        </form>";

                      // Добавляем обработчик события нажатия кнопки на C#
                      if (context.Request.Method == "POST" && context.Request.Form.ContainsKey("buyButton"))
                      {
                          // Получаем данные из формы

                          var bookId = context.Request.Form["bookId"];

                          work.DeletyBookFirstId(Convert.ToInt32(id));
                          context.Response.StatusCode = 200;
                          context.Response.ContentType = "text/html; charset=utf-8"; // Установка правильной кодировки
                          // Устанавливаем заголовок перенаправления
                          context.Response.Redirect("/");
                      }
                      else
                      {
                          context.Response.StatusCode = 200;
                          context.Response.ContentType = "text/html; charset=utf-8"; // Установка правильной кодировки
                          await context.Response.WriteAsync(formHtml);
                      }
                      // Отправляем HTML-форму в ответе
                  
                      //await context.Response.WriteAsJsonAsync(book);
                      //await context.Response.WriteAsync("Request starts with /book/");
              });
             
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllers(); // Добавление маршрутов для контроллеров API

            app.MapRazorPages();

            app.Run();
        }
    }
}
