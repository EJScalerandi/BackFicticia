using Microsoft.AspNetCore.Mvc;
using FicticiaApi.Data;
using FicticiaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly AppDbContext _context;

    public PersonasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
    {
        return await _context.Personas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetPersona(int id)
    {
        var persona = await _context.Personas.FindAsync(id);

        if (persona == null)
        {
            return NotFound();
        }

        return persona;
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> CreatePersona(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersona(int id, Persona persona)
    {
        if (id != persona.Id)
        {
            return BadRequest(new { message = "El ID de la persona no coincide con el ID proporcionado." });
        }

        _context.Entry(persona).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Personas.Any(e => e.Id == id))
            {
                return NotFound(new { message = "Persona no encontrada." });
            }
            else
            {
                throw;
            }
        }

        return Ok(new { message = "Persona actualizada correctamente." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersona(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null)
        {
            return NotFound(new { message = "Persona no encontrada." });
        }

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Persona eliminada correctamente." });
    }

    // Nuevo endpoint para registrar un usuario asociado a una persona existente
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(User user)
    {
        // Verificar si la Persona asociada existe
        var persona = await _context.Personas.FindAsync(user.PersonaId);
        if (persona == null)
        {
            return BadRequest(new { message = "La Persona asociada no existe." });
        }

        // Verifica si el nombre de usuario ya está en uso
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return Conflict(new { message = "El nombre de usuario ya está en uso." });
        }

        // Crear el nuevo usuario
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, user);
    }

    // Nuevo endpoint para login de usuario y retorno de la Persona asociada
    [HttpPost("login")]
    public async Task<IActionResult> Login(User loginRequest)
    {
        // Verifica si el usuario existe y la contraseña coincide
        var user = await _context.Users
            .Include(u => u.Persona) // Incluir la información de Persona
            .FirstOrDefaultAsync(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Credenciales incorrectas." });
        }

        // Genera el token JWT (si estás usando autenticación JWT), aquí solo es un placeholder
        var token = "fake-jwt-token"; // Cambia esto por tu lógica de generación de token si aplica

        // Retorna el token junto con la información de la persona asociada
        return Ok(new
        {
            token,
            userId = user.Id,
            username = user.Username,
            persona = new
            {
                user.Persona.Id,
                user.Persona.NombreCompleto,  // Aquí se reemplazó "Nombre" por "NombreCompleto"
                user.Persona.Edad,
                user.Persona.Genero,
                // Agrega más campos según sea necesario
            }
        });
    }

    [HttpGet("all-users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }
}
