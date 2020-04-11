using System;

namespace WebServiceTestImaginamos.Entities
{
    public class TransaccionEntity
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public long IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string NombreProducto { get; set; }
    }
}