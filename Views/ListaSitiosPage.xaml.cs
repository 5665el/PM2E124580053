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
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Sitio sitio) return;

        _sitioSeleccionado = sitio;
        collectionSitios.SelectedItem = null;

        bool irAlMapa = await DisplayAlert("Acción", "Desea ir a la ubicación indicada", "Yes", "No");
        if (irAlMapa)
        {
            await Navigation.PushAsync(new MapaPage(_sitioSeleccionado));
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (_sitioSeleccionado == null)
        {
            await DisplayAlert("Atención", "Seleccione un sitio de la lista primero.", "OK");
            return;
        }

        await _dataService.EliminarSitio(_sitioSeleccionado);
        var sitios = await _dataService.ObtenerSitios();
        collectionSitios.ItemsSource = sitios;
        _sitioSeleccionado = null;
    }

    private async void OnVerMapaClicked(object sender, EventArgs e)
    {
        if (_sitioSeleccionado == null)
        {
            await DisplayAlert("Atención", "Seleccione un sitio de la lista primero.", "OK");
            return;
        }

        await Navigation.PushAsync(new MapaPage(_sitioSeleccionado));
    }
}