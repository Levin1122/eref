using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MySqlConnection _connection;

        public AuthorController(IConfiguration configuration, MySqlConnection connection)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("EmployeeAppCon");
            _connection = new MySqlConnection(connectionString);
        }

        // GET:
        [HttpGet]
        public IActionResult Get()
        {
            var Authors = new List<Author>();
            _connection.Open();
            
            try
            {
                var command = new MySqlCommand("USE Prüfungdb; SELECT * FROM Author", _connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var Author = new Author()
                    {
                        AuthorId = reader.GetInt32("AuthorId"),
                        Nachname = reader.GetString("Nachname"),
                        Vorname = reader.GetString("Vorname"),
                        Stadt = reader.GetString("Stadt"),
                        Strasse = reader.GetString("Strasse")
                    };
                    Authors.Add(Author);
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
            
            return Ok(Authors);
        }

        // GET:
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _connection.Open();
            
            try
            {
                var command = new MySqlCommand("USE Prüfungdb; SELECT * FROM Author WHERE AuthorId = @AuthorId", _connection);
                command.Parameters.AddWithValue("@AuthorId", id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var Author = new Author()
                    {
                        AuthorId = reader.GetInt32("AuthorId"),
                        Nachname = reader.GetString("Nachname"),
                        Vorname = reader.GetString("Vorname"),
                        Stadt = reader.GetString("Stadt"),
                        Strasse = reader.GetString("Strasse")
                    };
                    return Ok(Author);
                }
                else
                {
                    return NotFound($"Author with ID {id} not found");
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

        // POST:
        [HttpPost]
        public IActionResult Post([FromBody] Author Author)
        {
            if (Author == null)
            {
                return BadRequest("Author object is null");
            }

            _connection.Open();

            try
            {
                using (var command = new MySqlCommand("USE Prüfungdb; INSERT INTO Author (Nachname, Vorname, Stadt, Strasse) VALUES (@Nachname, @Vorname, @Stadt, @Strasse)", _connection))
                {
                    command.Parameters.AddWithValue("@Nachname", Author.Nachname);
                    command.Parameters.AddWithValue("@Vorname", Author.Vorname);
                    command.Parameters.AddWithValue("@Stadt", Author.Stadt);
                    command.Parameters.AddWithValue("@Strasse", Author.Strasse);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok("Author added successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to add Ahuthor");
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

        // PUT:
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Author Author)
        {
            if (Author == null)
            {
                return BadRequest("Author object is null");
            }

            _connection.Open();

            try
            {
                using (var command = new MySqlCommand("USE Prüfungdb; UPDATE Author SET Nachname = @Nachname, Vorname = @Vorname, Stadt = @Stadt, Strasse = @Strasse WHERE AuthorId = @AuthorId", _connection))
                {
                    command.Parameters.AddWithValue("@AuthorId", id);
                    command.Parameters.AddWithValue("@Nachname", Author.Nachname);
                    command.Parameters.AddWithValue("@Vorname", Author.Vorname);
                    command.Parameters.AddWithValue("@Stadt", Author.Stadt);
                    command.Parameters.AddWithValue("@Strasse", Author.Strasse);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok($"Author with ID {id} updated successfully");
                    }
                    else
                    {
                        return NotFound($"Author with ID {id} not found");
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

        // DELETE:
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _connection.Open();

            try
            {
                using (var command = new MySqlCommand("USE Prüfungdb; DELETE FROM Author WHERE AuthorId = @AuthorId", _connection))
                {
                    command.Parameters.AddWithValue("@AuthorId", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok($"Author with ID {id} deleted successfully");
                    }
                    else
                    {
                        return NotFound($"Author with ID {id} not found");
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
}
