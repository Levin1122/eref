using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly MySqlConnection _connection;
    
    //Konstruktor
    public BookController(IConfiguration configuration, MySqlConnection connection)
    {
        _configuration = configuration;
        string connectionString = _configuration.GetConnectionString("EmployeeAppCon");
        _connection = new MySqlConnection(connectionString);
    }
     
    // GET
    [HttpGet]
    public OkObjectResult Get()
    {
        var departments = new List<Book>();
        _connection.Open();
        using (var command = new MySqlCommand("USE Prüfungdb; SELECT * FROM Prüfungdb.Book", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var depart = new Book()
                    {
                        BookId = reader.GetInt32("BookId"),
                        BookName = reader.GetString("BookName"),
                        // Map other fields...
                    };
                    departments.Add(depart);
                }
            }
        }
        _connection.Close();
        return Ok(departments);
    }
    
    // POST
    [HttpPost]
    public IActionResult Post([FromBody] Book Book)
    {
        if (Book == null)
        {
            return BadRequest("Book object is null");
        }

        _connection.Open();
    
        try
        {
            using (var command = new MySqlCommand("USE Prüfungdb; INSERT INTO Prüfungdb.Book (BookName) VALUES (@BookName)", _connection))
            {
                command.Parameters.AddWithValue("@BookName", Book.BookName);
                // Add other parameters as needed
            
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Book added successfully");
                }
                else
                {
                    return BadRequest("Failed to add Book");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
    
    // PUT
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Book Book)
    {
        if (Book == null)
        {
            return BadRequest("Book object is null");
        }

        _connection.Open();
    
        try
        {
            using (var command = new MySqlCommand("USE Prüfungdb; UPDATE Prüfungdb.Book SET BookName = @BookName WHERE BookId = @BookId", _connection))
            {
                command.Parameters.AddWithValue("@BookName", Book.BookName);
                command.Parameters.AddWithValue("@BookId", id);
                // Add other parameters as needed
            
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok($"Book with ID {id} updated successfully");
                }
                else
                {
                    return NotFound($"Book with ID {id} not found");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
    
    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _connection.Open();
    
        try
        {
            using (var command = new MySqlCommand("USE Prüfungdb; DELETE FROM Prüfungdb.Book WHERE BookId = @BookId", _connection))
            {
                command.Parameters.AddWithValue("@BookId", id);
            
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok($"Book with ID {id} deleted successfully");
                }
                else
                {
                    return NotFound($"Book with ID {id} not found");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }


    
 
}