using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private bool _isSelected;

        [Ignore] 
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
