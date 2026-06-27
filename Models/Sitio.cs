using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace PM2E124580053.Models
{
    public class Sitio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Imagen { get; set; }      

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Descripcion { get; set; }
    }
}