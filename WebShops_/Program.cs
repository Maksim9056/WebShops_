using System;

namespace WebShops_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();
            Operations.Operations work = new Operations.Operations();
            work.Start();

            work.Add();
            bool d = false;

    
            // Обработка запроса, если URL начинается с "/book/"
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/book"), appBuilder =>
            {
                appBuilder.Run(async context =>
                  {
                      // HTML-форма для ввода данных о книге

                      // Здесь вы можете добавить логику для обработки запросов, начинающихся с "/book/"
                      // Например, перенаправление на другой URL или возврат данных о книге в зависимости от запроса
                      //var book = work.SelectBookFirstId(id);
                      //if (book == null)
                      //{
                      //    context.Response.StatusCode = 404;
                      //    await context.Response.WriteAsync("Book not found");
                      //    return;
                      //}
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
                      // Пример:
                      //await context.Response.WriteAsync("Request starts with /book/");
              });
             
            });


            app.MapWhen(context => context.Request.Path.StartsWithSegments("/"), appBuilder =>
            {

                app.Run(async (context) =>
                {
                    var stringBuilder = new System.Text.StringBuilder();
                    d = true;
                    //app.MapGet("/book/{id:int}", async (HttpContext context, int id) =>
                    //{
                    //    // Здесь вы можете добавить логику для получения данных о книге с указанным id
                    //    // Затем возвращайте соответствующий ответ в зависимости от вашей логики

                    //    // Пример:
                    //    var book = work.SelectBookFirstId(id);
                    //    if (book == null)
                    //    {
                    //        d = true;
                    //        context.Response.StatusCode = 404;
                    //        await context.Response.WriteAsync("Book not found");
                    //        return;
                    //    }

                    //    context.Response.StatusCode = 200;
                    //    await context.Response.WriteAsJsonAsync(book); // Предположим, что book это объект книги
                    //});

                    // Если это не POST-запрос (не отправлена форма), то отображаем список всех книг
                    if (context.Request.Method != "POST")
                    {
                        //app.MapGet("/book/{id}", async (HttpContext context) =>
                        //{
                        //    var idString = context.Request.RouteValues["id"]?.ToString();
                        //    if (int.TryParse(idString, out int id))
                        //    {
                        //        // Перенаправляем на маршрут с числовым id
                        //        context.Response.Redirect($"/book/{id}");
                        //    }
                        //    else
                        //    {
                        //        context.Response.StatusCode = 400;
                        //        await context.Response.WriteAsync("Invalid book ID");
                        //    }
                        //});

                        // CSS стили для центрирования и размера таблицы
                        stringBuilder.Append("<style>");
                        stringBuilder.Append(".container { display: flex; flex-wrap: wrap; justify-content: space-between; }"); // Flexbox контейнер
                        stringBuilder.Append(".table-container { flex-basis: 40%; overflow: auto; }"); // Ширина таблицы 100%, добавлен скролл
                        stringBuilder.Append(".form-container { flex-basis: 40%; }"); // Ширина формы 100%
                        stringBuilder.Append("table { width: 90%; border-collapse: collapse; }"); // Ширина таблицы 100% и объединение границ
                        stringBuilder.Append("table, th, td { border: 1px solid black; padding: 8px; }"); // Границы и отступы для ячеек и заголовков
                        stringBuilder.Append(".center { text-align: center; }"); // Центрирование текста
                        stringBuilder.Append("@media (max-width: 768px) {"); // Медиа-запрос для экранов шириной до 768px
                        stringBuilder.Append(".container, .form-container { flex-direction: column; align-items: stretch; }"); // Один столбец
                        stringBuilder.Append(".table-container, .form-container { flex-basis: auto; margin-bottom: 20px; }"); // Автоматическая ширина
                        stringBuilder.Append(".form-container > div { flex-basis: 20%; margin-bottom: 10px; }"); // Увеличиваем ширину элементов формы и добавляем отступ
                        stringBuilder.Append("}");
                        stringBuilder.Append("</style>");


                        // Обертка для контейнера
                        stringBuilder.Append("<div class=\"container\">");

                        // Обертка для таблицы с книгами
                        stringBuilder.Append("<div class=\"table-container\">");

                        // Название "All Books"
                        stringBuilder.Append("<h2 class=\"center\">All Books</h2>");

                        // Таблица с книгами
                        stringBuilder.Append("<table>");

                        var allBooks = work.SelectBook();
                        foreach (var book in allBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a>></td><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }
                        stringBuilder.Append("</table>");

                        // Закрываем обертку для таблицы
                        stringBuilder.Append("</div>");

                        // Обертка для формы фильтрации
                        stringBuilder.Append("<div class=\"form-container\">");
                    }
                    else // Если была отправлена форма (POST-запрос), закрываем обертку для таблицы и начинаем форму фильтрации
                    {


                        var form = await context.Request.ReadFormAsync();
                        var author = form["author"];
                        var name = form["name"];
                        var date = form["date"];
                        var id = form["id"];
                        // Фильтрация записей по значениям фильтра
                        var filteredBooks = work.SelectBook().Where(book =>
                            (string.IsNullOrEmpty(author) || book.Author.Contains(author)) &&
                            (string.IsNullOrEmpty(name) || book.Name.Contains(name)) &&
                            (string.IsNullOrEmpty(date) || book.Year_of_publication.ToString().Contains(date)) &&
                            (string.IsNullOrEmpty(id) || book.Id.ToString().Contains(id))
                        );
                        stringBuilder.Append("<style>");
                        stringBuilder.Append("table { width: 30%; border-collapse: collapse; font-size: 0.8em; }"); // Уменьшение размера шрифта и объединение границ
                        stringBuilder.Append("th, td { border: 1px solid black; padding: 5px; }"); // Границы и отступы для ячеек и заголовков
                        stringBuilder.Append("</style>");

                        stringBuilder.Append("<h2 style=\"text-align: center;\">Filtered Results</h2>"); // Применение стиля для заголовка
                        stringBuilder.Append("<table>"); // Применение стилей для таблицы
                        stringBuilder.Append("<tr><th>ID</th><th>Name</th><th>Author</th><th>Year of Publication</th></tr>"); // Заголовки столбцов
                        foreach (var book in filteredBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a></td><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }
                        stringBuilder.Append("</table>");
                        stringBuilder.Append("</div>");


                    }
                    // Форма фильтрации
                    stringBuilder.Append("<div style=\"background-color: #f9f9f9; border: 1px solid #ddd; padding: 5px; border-radius: 5px; margin-bottom: 5px;\">"); // Изменены значения padding и margin
                    stringBuilder.Append("<h3 style=\"text-align: center; margin-bottom: 5px;\">Фильтр</h3>"); // Изменено значение margin-bottom
                    stringBuilder.Append("<form method=\"post\" action=\"/\">");
                    stringBuilder.Append("<div style=\"display: flex; flex-wrap: wrap; justify-content: space-between;\">");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // Изменены значения flex-basis и margin-bottom
                    stringBuilder.Append("<label for=\"author\">Автор:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"author\" name=\"author\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // Изменены значения flex-basis и margin-bottom
                    stringBuilder.Append("<label for=\"name\">Название:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"name\" name=\"name\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // Изменены значения flex-basis и margin-bottom
                    stringBuilder.Append("<label for=\"date\">Дата:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"date\" name=\"date\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // Изменены значения flex-basis и margin-bottom
                    stringBuilder.Append("<label for=\"id\">ID:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"id\" name=\"id\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<button type=\"submit\" style=\"background-color: #4CAF50; color: white; padding: 5px 7.5px; border: none; border-radius: 4px; cursor: pointer; width: 40%;\">Применить</button>"); // Изменены значения padding
                    stringBuilder.Append("</form>");
                    // Кнопка "Показать все"
                    stringBuilder.Append("<form method=\"get\" action=\"/\">");
                    stringBuilder.Append("<button type=\"submit\" style=\"background-color: #f44336; color: white; padding: 5px 7.5px; border: none; border-radius: 4px; cursor: pointer; margin-top: 5px; width: 40%;\">Показать все</button>"); // Изменены значения padding и margin-top
                    stringBuilder.Append("</form>");
                    stringBuilder.Append("</div>");




                    if (context.Request.Method != "POST")
                    {
                        stringBuilder.Append("</div>"); // Закрываем обертку для контейнера
                    }

                    context.Response.ContentType = "text/html; charset=utf-8"; // Установка правильной кодировки
                    await context.Response.WriteAsync(stringBuilder.ToString());
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

            app.MapRazorPages();

            app.Run();
        }
    }
}
