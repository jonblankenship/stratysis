using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Stratysis.Wpf.ViewModels;

namespace Stratysis.Wpf
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

        public ParametersViewModel ParametersViewModel => App.ServiceProvider.GetRequiredService<ParametersViewModel>();

        public CandlestickChartViewModel CandlestickChartViewModel => App.ServiceProvider.GetRequiredService<CandlestickChartViewModel>();

    }
}
