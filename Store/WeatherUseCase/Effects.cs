using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;

public class Effects
{
    private readonly IWeatherForecastService WeatherForecastService;

    public Effects(IWeatherForecastService weatherForecastService)
    {
        WeatherForecastService = weatherForecastService;
    }

    [EffectMethod]
    public async Task HandleFetchDataAction(FetchDataAction action, IDispatcher dispatcher)
    {
        var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    }
}