namespace FicticiaApi.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Genero { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public bool? Maneja { get; set; }
        public bool? UsaLentes { get; set; }
        public bool? Diabetico { get; set; }
        public string OtraEnfermedad { get; set; } = string.Empty;
        public string AtributosAdicionales { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

