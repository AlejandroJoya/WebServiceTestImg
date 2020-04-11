namespace WebServiceTestImaginamos.Entities
{
    public class UsuarioEntity
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }

        public UsuarioEntity() { }

        public UsuarioEntity(Usuario usuario)
        {
            if (usuario == null)
            {
                return;
            }

            Id = usuario.Id;
            Nombre = usuario.Nombre;
            Documento = usuario.Documento;
            Email = usuario.Email;
            Contrasena = usuario.Contrasena;
            NombreUsuario = usuario.NombreUsuario;
        }
    }
}