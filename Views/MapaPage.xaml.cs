using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var ubicacion = new Location(_sitio.Latitud, _sitio.Longitud);

        mapaSitio.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacion, Distance.FromKilometers(1)));

        mapaSitio.Pins.Clear();
        mapaSitio.Pins.Add(new Pin
        {
            Location = ubicacion,
            Label = "Pin: " + _sitio.Descripcion,
            Type = PinType.Place
        });

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("GPS inactivo", "Active la ubicación para ver su posición en el mapa.", "OK");
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