namespace ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
public class FetchDataResultAction
{
    public IEnumerable<WeatherForecast> Forecasts { get; }

    public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
    {
        Forecasts = forecasts;
    }
}