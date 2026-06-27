using System;
using System.Collections.Generic;
using System.Text;
using PM2E124580053.Models;
using SQLite;

namespace PM2E124580053.Services
{
    public class DataService
    {
        private SQLiteAsyncConnection _conexion;

        public DataService()
        {
            var rutaBD = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "sitios.db3");

            _conexion = new SQLiteAsyncConnection(rutaBD);
            _conexion.CreateTableAsync<Sitio>().Wait();
        }

        public Task<int> GuardarSitio(Sitio sitio)
        {
            return _conexion.InsertAsync(sitio);
        }

        public Task<List<Sitio>> ObtenerSitios()
        {
            return _conexion.Table<Sitio>().ToListAsync();
        }

        public Task<int> EliminarSitio(Sitio sitio)
        {
            return _conexion.DeleteAsync(sitio);
        }
    }
}