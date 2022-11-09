using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Dependencias a usar:
using KenyWebAPI.Modelos;
using System.Data;
using System.Data.SqlClient;

namespace KenyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration config){
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {

            List<Producto> lista = new List<Producto>();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }

                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });

            }
        }
        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {

            List<Producto> lista = new List<Producto>();
            Producto oproducto = new Producto();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }

                    }
                }

                oproducto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oproducto });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = oproducto });

            }
        }

        //***** POST
        //GUARDAR UN PRODUCTO A LA LISTA 
        //==> http://localhost:5124/api/Guardar
        //Json => nombre,marca,categoria,precio => { ("nombre" = : "test", "marca" : "test") }
        //*****

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        //***** PUT
        //EDITAR UN PRODUCTO DE LA LISTA ESPECIFICANDO LA ID EN EL JSON
        //==> http://localhost:5124/api/Editar
        //Json => id,nombre,marca,categoria,precio => { ("idProducto" = : 13, "nombre" : "test2") }
        //*****

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }


        //***** DELETE
        //ELIMINAR UN PRODUCTO DE LA LISTA ESPISIFICANDO LA EN LA URL
        //==> http://localhost:5124/api/Eliminar/13
        //*****

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }




    }
}
