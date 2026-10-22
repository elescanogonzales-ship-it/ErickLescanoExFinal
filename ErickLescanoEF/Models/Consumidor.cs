using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErickLescanoEF.Models
{
    public class Consumidor
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }

        public string Pais { get; set; }

        public Consumidor()
        {

        }

        public Consumidor(string dni, string nombre, string apellido, string email, string pais)
        {
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Pais = pais;
        }
    }
}