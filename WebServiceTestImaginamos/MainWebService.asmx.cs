using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using WebServiceTestImaginamos.Entities;

namespace WebServiceTestImaginamos
{
    /// <summary>
    /// Summary description for MainWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MainWebService : WebService
    {
        private TestImaginamosEntities context = new TestImaginamosEntities();

        [WebMethod]
        public bool Login(string usuario, string contrasena)
        {
            try
            {
                bool existeUsuario = context.Usuarios.Any(m => m.NombreUsuario.Equals(usuario) && m.Contrasena.Equals(contrasena));
                return existeUsuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public string RegistrarUsuario(string nombre, string documento, string email, string usuario, string contrasena)
        {
            try
            {
                var usuarioModelo = new Usuario();
                usuarioModelo.Nombre = nombre;
                usuarioModelo.Documento = documento;
                usuarioModelo.Email = email;
                usuarioModelo.NombreUsuario = usuario;
                usuarioModelo.Contrasena = contrasena;
                context.Usuarios.Add(usuarioModelo);

                context.SaveChanges();

                return MensajeUsuario.REGISTRO_EXITOSO;
            }
            catch (Exception ex)
            {
                return MensajeUsuario.REGISTRO_FALLIDO + " " + ex.Message;
                throw;
            }
        }

        [WebMethod]
        public List<ProductoEntity> ListarProductos()
        {
            try
            {

                var listaProductos = (from producto in context.Productoes
                                      orderby producto.NombreProducto
                                      select new
                                      {
                                          producto.Id,
                                          producto.NombreProducto,
                                          producto.Cantidad,
                                          producto.Precio
                                      }
                                      )
                                    .ToList();

                var listaRetorno = new List<ProductoEntity>();

                foreach (var item in listaProductos)
                {
                    listaRetorno.Add(new ProductoEntity
                    {
                        Id = item.Id,
                        NombreProducto = item.NombreProducto,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio
                    });
                }

                return listaRetorno;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public string AgregarProductoCarrito(long idUsuario, long idProducto)
        {
            try
            {
                var transaccion = new Transaccion();
                transaccion.IdProducto = idProducto;
                transaccion.IdUsuario = idUsuario;
                transaccion.Fecha = DateTime.Now;
                transaccion.Estado = EstadoTransaccion.NUEVO.ToString();

                context.Transaccions.Add(transaccion);
                context.SaveChanges();

                return MensajeUsuario.REGISTRO_EXITOSO;
            }
            catch (Exception ex)
            {
                return MensajeUsuario.REGISTRO_FALLIDO + " " + ex.Message;
                throw;
            }
        }

        [WebMethod]
        public string AgregarProducto(string nombre, decimal precio, int cantidad)
        {
            try
            {
                var producto = new Producto();
                producto.NombreProducto = nombre;
                producto.Precio = precio;
                producto.Cantidad = cantidad;

                context.Productoes.Add(producto);
                context.SaveChanges();

                return MensajeUsuario.REGISTRO_EXITOSO;
            }
            catch (Exception ex)
            {
                return MensajeUsuario.REGISTRO_FALLIDO + " " + ex.Message;
                throw;
            }
        }

        [WebMethod]
        public ProductoEntity ConsultarProducto(long id)
        {
            try
            {
                var producto = context.Productoes.Find(id);
                var productoEntidad = new ProductoEntity
                {
                    Id = producto.Id,
                    NombreProducto = producto.NombreProducto,
                    Precio = producto.Precio,
                    Cantidad = producto.Cantidad
                };

                return productoEntidad;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public List<TransaccionEntity> ListarCarrito(long idUsuario)
        {
            try
            {
                string estadoNuevo = EstadoTransaccion.NUEVO.ToString();

                var listaCarrito = (from transaccion in context.Transaccions
                                    orderby transaccion.Id
                                    where transaccion.IdUsuario == idUsuario && transaccion.Estado.ToUpper().Equals(estadoNuevo)
                                    select new
                                    {
                                        transaccion.Id,
                                        transaccion.IdUsuario,
                                        transaccion.IdProducto,
                                        transaccion.Fecha,
                                        transaccion.Estado,
                                        transaccion.Producto.NombreProducto
                                    }
                                      )
                                    .ToList();

                var listaRetorno = new List<TransaccionEntity>();

                foreach (var item in listaCarrito)
                {
                    listaRetorno.Add(new TransaccionEntity
                    {
                        Id = item.Id,
                        IdUsuario = item.IdUsuario,
                        IdProducto = item.IdProducto,
                        Fecha = item.Fecha,
                        Estado = item.Estado,
                        NombreProducto = item.NombreProducto
                    });
                }

                return listaRetorno;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
