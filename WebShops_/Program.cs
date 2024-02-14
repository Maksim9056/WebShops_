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

    
            // ��������� �������, ���� URL ���������� � "/book/"
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/book"), appBuilder =>
            {
                appBuilder.Run(async context =>
                  {
                      // HTML-����� ��� ����� ������ � �����

                      // ����� �� ������ �������� ������ ��� ��������� ��������, ������������ � "/book/"
                      // ��������, ��������������� �� ������ URL ��� ������� ������ � ����� � ����������� �� �������
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
                      // ��������� HTML-�����
                      var formHtml = $@"<style>/* ����� ��� ����� */ form {{
                background-color: #f9f9f9;
                border: 1px solid #ddd;
                padding: 10px;
                border-radius: 5px;
                margin-bottom: 10px;
                width: 300px; /* ������ ����� */
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
            <button type='submit' name='buyButton'>������</button> <!-- �������� type �� submit � �������� name ��� ������ -->
            <input type='hidden' name='bookId' value='{id}'> <!-- �������� ID ����� ��� ������� ���� -->
        </form>";

                      // ��������� ���������� ������� ������� ������ �� C#
                      if (context.Request.Method == "POST" && context.Request.Form.ContainsKey("buyButton"))
                      {
                          // �������� ������ �� �����
                          var bookId = context.Request.Form["bookId"];
                          context.Response.StatusCode = 200;
                          context.Response.ContentType = "text/html; charset=utf-8"; // ��������� ���������� ���������
                          // ������������� ��������� ���������������
                          context.Response.Redirect("/");
                      }
                      else
                      {
                          context.Response.StatusCode = 200;
                          context.Response.ContentType = "text/html; charset=utf-8"; // ��������� ���������� ���������
                          await context.Response.WriteAsync(formHtml);
                      }
                      // ���������� HTML-����� � ������
                  
                      //await context.Response.WriteAsJsonAsync(book);
                      // ������:
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
                    //    // ����� �� ������ �������� ������ ��� ��������� ������ � ����� � ��������� id
                    //    // ����� ����������� ��������������� ����� � ����������� �� ����� ������

                    //    // ������:
                    //    var book = work.SelectBookFirstId(id);
                    //    if (book == null)
                    //    {
                    //        d = true;
                    //        context.Response.StatusCode = 404;
                    //        await context.Response.WriteAsync("Book not found");
                    //        return;
                    //    }

                    //    context.Response.StatusCode = 200;
                    //    await context.Response.WriteAsJsonAsync(book); // �����������, ��� book ��� ������ �����
                    //});

                    // ���� ��� �� POST-������ (�� ���������� �����), �� ���������� ������ ���� ����
                    if (context.Request.Method != "POST")
                    {
                        //app.MapGet("/book/{id}", async (HttpContext context) =>
                        //{
                        //    var idString = context.Request.RouteValues["id"]?.ToString();
                        //    if (int.TryParse(idString, out int id))
                        //    {
                        //        // �������������� �� ������� � �������� id
                        //        context.Response.Redirect($"/book/{id}");
                        //    }
                        //    else
                        //    {
                        //        context.Response.StatusCode = 400;
                        //        await context.Response.WriteAsync("Invalid book ID");
                        //    }
                        //});

                        // CSS ����� ��� ������������� � ������� �������
                        stringBuilder.Append("<style>");
                        stringBuilder.Append(".container { display: flex; flex-wrap: wrap; justify-content: space-between; }"); // Flexbox ���������
                        stringBuilder.Append(".table-container { flex-basis: 40%; overflow: auto; }"); // ������ ������� 100%, �������� ������
                        stringBuilder.Append(".form-container { flex-basis: 40%; }"); // ������ ����� 100%
                        stringBuilder.Append("table { width: 90%; border-collapse: collapse; }"); // ������ ������� 100% � ����������� ������
                        stringBuilder.Append("table, th, td { border: 1px solid black; padding: 8px; }"); // ������� � ������� ��� ����� � ����������
                        stringBuilder.Append(".center { text-align: center; }"); // ������������� ������
                        stringBuilder.Append("@media (max-width: 768px) {"); // �����-������ ��� ������� ������� �� 768px
                        stringBuilder.Append(".container, .form-container { flex-direction: column; align-items: stretch; }"); // ���� �������
                        stringBuilder.Append(".table-container, .form-container { flex-basis: auto; margin-bottom: 20px; }"); // �������������� ������
                        stringBuilder.Append(".form-container > div { flex-basis: 20%; margin-bottom: 10px; }"); // ����������� ������ ��������� ����� � ��������� ������
                        stringBuilder.Append("}");
                        stringBuilder.Append("</style>");


                        // ������� ��� ����������
                        stringBuilder.Append("<div class=\"container\">");

                        // ������� ��� ������� � �������
                        stringBuilder.Append("<div class=\"table-container\">");

                        // �������� "All Books"
                        stringBuilder.Append("<h2 class=\"center\">All Books</h2>");

                        // ������� � �������
                        stringBuilder.Append("<table>");

                        var allBooks = work.SelectBook();
                        foreach (var book in allBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a>></td><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }
                        stringBuilder.Append("</table>");

                        // ��������� ������� ��� �������
                        stringBuilder.Append("</div>");

                        // ������� ��� ����� ����������
                        stringBuilder.Append("<div class=\"form-container\">");
                    }
                    else // ���� ���� ���������� ����� (POST-������), ��������� ������� ��� ������� � �������� ����� ����������
                    {


                        var form = await context.Request.ReadFormAsync();
                        var author = form["author"];
                        var name = form["name"];
                        var date = form["date"];
                        var id = form["id"];
                        // ���������� ������� �� ��������� �������
                        var filteredBooks = work.SelectBook().Where(book =>
                            (string.IsNullOrEmpty(author) || book.Author.Contains(author)) &&
                            (string.IsNullOrEmpty(name) || book.Name.Contains(name)) &&
                            (string.IsNullOrEmpty(date) || book.Year_of_publication.ToString().Contains(date)) &&
                            (string.IsNullOrEmpty(id) || book.Id.ToString().Contains(id))
                        );
                        stringBuilder.Append("<style>");
                        stringBuilder.Append("table { width: 30%; border-collapse: collapse; font-size: 0.8em; }"); // ���������� ������� ������ � ����������� ������
                        stringBuilder.Append("th, td { border: 1px solid black; padding: 5px; }"); // ������� � ������� ��� ����� � ����������
                        stringBuilder.Append("</style>");

                        stringBuilder.Append("<h2 style=\"text-align: center;\">Filtered Results</h2>"); // ���������� ����� ��� ���������
                        stringBuilder.Append("<table>"); // ���������� ������ ��� �������
                        stringBuilder.Append("<tr><th>ID</th><th>Name</th><th>Author</th><th>Year of Publication</th></tr>"); // ��������� ��������
                        foreach (var book in filteredBooks)
                        {
                            stringBuilder.Append($"<tr><td>{book.Id}</td><td><a href=\"/book/{book.Id}\">{book.Name}</a></td><td>{book.Author}</td><td>{book.Year_of_publication}</td></tr>");
                        }
                        stringBuilder.Append("</table>");
                        stringBuilder.Append("</div>");


                    }
                    // ����� ����������
                    stringBuilder.Append("<div style=\"background-color: #f9f9f9; border: 1px solid #ddd; padding: 5px; border-radius: 5px; margin-bottom: 5px;\">"); // �������� �������� padding � margin
                    stringBuilder.Append("<h3 style=\"text-align: center; margin-bottom: 5px;\">������</h3>"); // �������� �������� margin-bottom
                    stringBuilder.Append("<form method=\"post\" action=\"/\">");
                    stringBuilder.Append("<div style=\"display: flex; flex-wrap: wrap; justify-content: space-between;\">");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // �������� �������� flex-basis � margin-bottom
                    stringBuilder.Append("<label for=\"author\">�����:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"author\" name=\"author\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // �������� �������� flex-basis � margin-bottom
                    stringBuilder.Append("<label for=\"name\">��������:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"name\" name=\"name\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // �������� �������� flex-basis � margin-bottom
                    stringBuilder.Append("<label for=\"date\">����:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"date\" name=\"date\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div style=\"flex-basis: calc(100% - 2.5px); margin-bottom: 5px;\">"); // �������� �������� flex-basis � margin-bottom
                    stringBuilder.Append("<label for=\"id\">ID:</label>");
                    stringBuilder.Append("<input type=\"text\" id=\"id\" name=\"id\" style=\"width: calc(100% - 22px);\"><br>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<button type=\"submit\" style=\"background-color: #4CAF50; color: white; padding: 5px 7.5px; border: none; border-radius: 4px; cursor: pointer; width: 40%;\">���������</button>"); // �������� �������� padding
                    stringBuilder.Append("</form>");
                    // ������ "�������� ���"
                    stringBuilder.Append("<form method=\"get\" action=\"/\">");
                    stringBuilder.Append("<button type=\"submit\" style=\"background-color: #f44336; color: white; padding: 5px 7.5px; border: none; border-radius: 4px; cursor: pointer; margin-top: 5px; width: 40%;\">�������� ���</button>"); // �������� �������� padding � margin-top
                    stringBuilder.Append("</form>");
                    stringBuilder.Append("</div>");




                    if (context.Request.Method != "POST")
                    {
                        stringBuilder.Append("</div>"); // ��������� ������� ��� ����������
                    }

                    context.Response.ContentType = "text/html; charset=utf-8"; // ��������� ���������� ���������
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
