using PM2E124580053.Models;

namespace PM2E124580053.Views;

public partial class MapaPage : ContentPage
{
    private readonly Sitio _sitio;

    public MapaPage(Sitio sitio)
    {
        InitializeComponent();
        _sitio = sitio;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblDescripcion.Text = _sitio.Descripcion;
        lblCoordenadas.Text = $"Lat: {_sitio.Latitud}  |  Lon: {_sitio.Longitud}";
    }

    private async void OnVerMapaClicked(object sender, EventArgs e)
    {
        try
        {
            var url = $"https://www.google.com/maps?q={_sitio.Latitud},{_sitio.Longitud}";
            await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnCompartirClicked(object sender, EventArgs e)
    {
        try
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir imagen del sitio",
                File = new ShareFile(_sitio.Imagen)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}