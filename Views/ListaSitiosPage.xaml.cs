using PM2E124580053.Models;
using PM2E124580053.Services;

namespace PM2E124580053.Views;

public partial class ListaSitiosPage : ContentPage
{
    private readonly DataService _dataService;
    private Sitio? _sitioSeleccionado;

    public ListaSitiosPage()
    {
        InitializeComponent();
        _dataService = new DataService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var sitios = await _dataService.ObtenerSitios();
        collectionSitios.ItemsSource = sitios;
        _sitioSeleccionado = null;
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox checkBox || checkBox.BindingContext is not Sitio sitio) return;

        if (e.Value)
        {
            // Desmarca el anterior, para que solo uno esté seleccionado a la vez
            if (_sitioSeleccionado != null && _sitioSeleccionado != sitio)
                _sitioSeleccionado.IsSelected = false;

            _sitioSeleccionado = sitio;
        }
        else if (_sitioSeleccionado == sitio)
        {
            _sitioSeleccionado = null;
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (_sitioSeleccionado == null)
        {
            await DisplayAlert("Atención", "Seleccione un sitio con el checkbox primero.", "OK");
            return;
        }

        bool confirmar = await DisplayAlert(
            "Confirmar eliminación",
            $"¿Desea eliminar el sitio \"{_sitioSeleccionado.Descripcion}\"?",
            "Sí",
            "No");

        if (!confirmar) return;

        await _dataService.EliminarSitio(_sitioSeleccionado);
        var sitios = await _dataService.ObtenerSitios();
        collectionSitios.ItemsSource = sitios;
        _sitioSeleccionado = null;
    }

    private async void OnVerMapaClicked(object sender, EventArgs e)
    {
        if (_sitioSeleccionado == null)
        {
            await DisplayAlert("Atención", "Seleccione un sitio con el checkbox primero.", "OK");
            return;
        }

        await Navigation.PushAsync(new MapaPage(_sitioSeleccionado));
    }
}