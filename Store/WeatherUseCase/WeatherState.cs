using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
[FeatureState]
public class WeatherState
{
    public bool IsLoading { get; }
    public IEnumerable<WeatherForecast> Forecasts { get; }

    private WeatherState() { } // Required for creating initial state

    public WeatherState(bool isLoading, IEnumerable<WeatherForecast> forecasts)
    {
        IsLoading = isLoading;
        Forecasts = forecasts ?? Array.Empty<WeatherForecast>();
    }
}
