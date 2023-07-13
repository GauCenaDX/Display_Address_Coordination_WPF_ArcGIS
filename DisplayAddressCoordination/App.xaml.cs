using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DisplayAddressCoordination
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK6317b8ed1f8142a6ba107ed477a2a00dHdIBjiprOMnpXEYjV8jl4zy-lwNm-CxOgkd95dTCgRG4LkYb_ryGu6FCKkEdZx2w";
        }
    }
}
