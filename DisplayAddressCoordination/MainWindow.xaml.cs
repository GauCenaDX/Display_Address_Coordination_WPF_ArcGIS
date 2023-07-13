using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;

namespace DisplayAddressCoordination
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeocodeResult _geocodeResult;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GeocodeButton_Click(object sender, RoutedEventArgs e)
        {
            string address = addressTextBox.Text;

            _geocodeResult = await GeocodeAddress(address);

            if (_geocodeResult != null)
            {
                latitudeTextBlock.Text = _geocodeResult.DisplayLocation.Y.ToString();
                longitudeTextBlock.Text = _geocodeResult.DisplayLocation.X.ToString();
            }
        }

        private async void ShowOnMapButton_Click(object sender, RoutedEventArgs e)
        {
            if (_geocodeResult == null)
            {
                MessageBox.Show("Please geocode an address first.");
                return;
            }

            await ShowOnMap(_geocodeResult.DisplayLocation.Y, _geocodeResult.DisplayLocation.X);
        }

        private async Task<GeocodeResult> GeocodeAddress(string address)
        {
            try
            {
                LocatorTask locatorTask = await LocatorTask.CreateAsync(new Uri("https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"));

                GeocodeParameters parameters = new GeocodeParameters { MaxResults = 1, OutputSpatialReference = SpatialReferences.Wgs84 };

                IReadOnlyList<GeocodeResult> results = await locatorTask.GeocodeAsync(address, parameters);

                if (results.Count > 0)
                {
                    return results[0];
                }
                else
                {
                    MessageBox.Show("No results found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return null;
        }

        private async Task ShowOnMap(double latitude, double longitude)
        {
            try
            {
                var mapView = new MapView();
                Map map = new Map(BasemapStyle.ArcGISStreets);
                mapView.Map = map;
                mapView.Map.InitialViewpoint = new Viewpoint(latitude, longitude, 10000);

                GraphicsOverlay graphicsOverlay = new GraphicsOverlay();
                mapView.GraphicsOverlays.Add(graphicsOverlay);

                // Convert the WPF color to an ArcGIS Runtime color
                System.Windows.Media.Color wpfColor = System.Windows.Media.Colors.Red;
                System.Drawing.Color arcgisColor = System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);

                SimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Cross, arcgisColor, 10);

                MapPoint location = new MapPoint(longitude, latitude, SpatialReferences.Wgs84);
                Graphic graphic = new Graphic(location, markerSymbol);
                graphicsOverlay.Graphics.Add(graphic);

                Window mapWindow = new Window
                {
                    Title = "Location Map",
                    Content = mapView,
                    Width = 800,
                    Height = 600
                };

                mapWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
