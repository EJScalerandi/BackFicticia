using System.ComponentModel.DataAnnotations.Schema;

namespace FicticiaApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Recuerda cifrar la contraseña en un entorno real

        // Relación con la tabla Personas
        [ForeignKey("Persona")]
        public int PersonaId { get; set; }

        // Referencia a la entidad Persona
        public Persona? Persona { get; set; } = new Persona(); // Inicializado para evitar advertencias
    }
}
