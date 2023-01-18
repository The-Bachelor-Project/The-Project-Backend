using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

TestConnection test = new TestConnection();
test.testConnection();

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();

public class TestConnection
{
    public void testConnection()
    {
        // Replace with your actual connection string
        string connectionString = "Server=tcp:stock-database-server.database.windows.net,1433;Initial Catalog=stock_app_db;Persist Security Info=False;User ID=bachelor;Password=Gustav.Frederik;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Connection to the database is successful!");

            // Test the SELECT statement
            string queryString = "SELECT * FROM Account";
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Process the result set
                    while (reader.Read())
                    {
                        Console.WriteLine("Account ID: " + reader[0] + ", Account Name: " + reader[1]);
                    }
                }
            }
        }
    }
}
