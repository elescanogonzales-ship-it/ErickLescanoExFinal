using ErickLescanoEF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErickLescanoEF.Controllers
{
    public class ConsumidorController : Controller
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["conexionDatos"].ConnectionString;

        /* obtener lista de consumidores */
        IEnumerable<Consumidor> obtenerConsumidores()
        {
            List<Consumidor> listarConsumidor= new List<Consumidor>();
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("usp_listar_consumidor", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listarConsumidor.Add(
                    new Consumidor
                    {
                        Dni = sqlDataReader.GetString(0),
                        Nombre = sqlDataReader.GetString(1),
                        Apellido = sqlDataReader.GetString(2),
                        Email = sqlDataReader.GetString(3),
                        Pais = sqlDataReader.GetString(4),
                    });
            }
            return listarConsumidor;
        }


        /* Agregar Consumidor */
        string agregarConsumidoro(Consumidor consumidor)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("usp_agrega_consumidor", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@dnicont", consumidor.Dni);
            sqlCommand.Parameters.AddWithValue("@nomcont", consumidor.Nombre);
            sqlCommand.Parameters.AddWithValue("@apecont", consumidor.Apellido);
            sqlCommand.Parameters.AddWithValue("@emailcont", consumidor.Email);
            sqlCommand.Parameters.AddWithValue("@idpais", consumidor.Pais);


            SqlParameter paramResultado = new SqlParameter("@resultado", System.Data.SqlDbType.Int);
            paramResultado.Direction = System.Data.ParameterDirection.Output;
            sqlCommand.Parameters.Add(paramResultado);

           
            int cantidadFilas = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
            return cantidadFilas > 0 ? "Dato guardado" : "Error";
        }


        Consumidor obtenerConsumidorDni(string dni)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("sp_obtenerConsumidorDni", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Dni", dni);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                return new Consumidor
                {
                    Dni = sqlDataReader.GetString(0),
                    Nombre = sqlDataReader.GetString(1),
                    Apellido = sqlDataReader.GetString(2),
                    Email = sqlDataReader.GetString(3),
                    Pais = sqlDataReader.GetString(4),
                };
            }
            ;
            return null;
        }


        int  eliminarConsumidor(string dni)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("usp_eliminar_consumidor", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@dnicont", dni);

            return sqlCommand.ExecuteNonQuery();
        }



        string editarConsumidor(Consumidor consumidor)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("usp_editar_consumidor", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

           sqlCommand.Parameters.AddWithValue("@dnicont", consumidor.Dni);
            sqlCommand.Parameters.AddWithValue("@nomcont", consumidor.Nombre);
            sqlCommand.Parameters.AddWithValue("@apecont", consumidor.Apellido);
            sqlCommand.Parameters.AddWithValue("@emailcont", consumidor.Email);
            sqlCommand.Parameters.AddWithValue("@idpais", consumidor.Pais);

            SqlParameter paramResultado = new SqlParameter("@resultado", System.Data.SqlDbType.Int);
            paramResultado.Direction = System.Data.ParameterDirection.Output;
            sqlCommand.Parameters.Add(paramResultado);


            int cantidadFilas = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
            return cantidadFilas > 0 ? "Dato actualizado" : "Error";


        }


        public ActionResult Index()
        {
            return View(obtenerConsumidores());
        }


        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Consumidor consumidor)
        {
            ViewBag.Mensaje = agregarConsumidoro(consumidor);
            return View(consumidor);
        }



        public ActionResult Delete(string dni)
        {
            Consumidor consumidorEncontradoo = obtenerConsumidorDni(dni);
            return View(consumidorEncontradoo);
        }

        [HttpPost]
        public ActionResult Delete(Consumidor consumidor)
        {
            int cantidadCambios = eliminarConsumidor(consumidor.Dni);
            if (cantidadCambios > 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Details(string dni)
        {
            Consumidor consumidorEncontradoo = obtenerConsumidorDni(dni);
            return View(consumidorEncontradoo);
        }



        public ActionResult Edit(string dni)
        {
            Consumidor consumidorEncontradoo = obtenerConsumidorDni(dni);
            return View(consumidorEncontradoo);
        }


        [HttpPost]
        public ActionResult Edit(Consumidor consumidor)
        {

            string mensaje = editarConsumidor(consumidor);
            ViewBag.Mensaje = mensaje;

            return View(consumidor);


        }
    }
}