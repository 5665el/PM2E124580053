using PM2E124580053.Models;
using PM2E124580053.Services;
using PM2E124580053.Views;


namespace PM2E124580053
{
    public partial class MainPage : ContentPage
    {
        private readonly DataService _dataService;
        private string? _rutaFotoActual;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new DataService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ObtenerUbicacionAsync();
        }

        private async System.Threading.Tasks.Task ObtenerUbicacionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("GPS inactivo", "Active el permiso de ubicación para continuar.", "OK");
                    return;
                }

                var location = await Geolocation.GetLastKnownLocationAsync()
                               ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                if (location == null)
                {
                    await DisplayAlert("GPS inactivo", "No se pudo obtener su ubicación. Verifique que el GPS esté activo.", "OK");
                    return;
                }

                entryLatitud.Text = location.Latitude.ToString();
                entryLongitud.Text = location.Longitude.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de GPS", ex.Message, "OK");
            }
        }

        private async void OnTomarFotoClicked(object sender, EventArgs e)
        {
            try
            {
                if (!MediaPicker.IsCaptureSupported)
                {
                    await DisplayAlert("Error", "La cámara no está disponible en este dispositivo.", "OK");
                    return;
                }

                var foto = await MediaPicker.CapturePhotoAsync();
                if (foto != null)
                {
                    var rutaLocal = System.IO.Path.Combine(FileSystem.CacheDirectory, foto.FileName);
                    using (var stream = await foto.OpenReadAsync())
                    using (var nuevoArchivo = System.IO.File.OpenWrite(rutaLocal))
                    {
                        await stream.CopyToAsync(nuevoArchivo);
                    }

                    _rutaFotoActual = rutaLocal;
                    imgSitio.Source = ImageSource.FromFile(rutaLocal);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            // Validaciones de input del usuario
            if (string.IsNullOrEmpty(_rutaFotoActual))
            {
                await DisplayAlert("Validación", "Debe tomar una foto del sitio.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(entryLatitud.Text) || string.IsNullOrEmpty(entryLongitud.Text))
            {
                await DisplayAlert("Validación", "No se obtuvo latitud/longitud. Verifique el GPS.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(editorDescripcion.Text))
            {
                await DisplayAlert("Validación", "Debe ingresar una descripción.", "OK");
                return;
            }

            var sitio = new Sitio
            {
                Imagen = _rutaFotoActual,
                Latitud = double.Parse(entryLatitud.Text),
                Longitud = double.Parse(entryLongitud.Text),
                Descripcion = editorDescripcion.Text
            };

            await _dataService.GuardarSitio(sitio);
            await DisplayAlert("Éxito", "Sitio guardado correctamente.", "OK");

            // Limpiar formulario
            imgSitio.Source = null;
            _rutaFotoActual = null;
            editorDescripcion.Text = string.Empty;
        }

        private async void OnListaSitiosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaSitiosPage());
        }

        private void OnSalirClicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}