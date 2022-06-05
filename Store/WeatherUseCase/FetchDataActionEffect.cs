namespace ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
public class FetchDataActionEffect : Effect<FetchDataAction>
{
    private readonly IWeatherForecastService WeatherForecastService;

    public FetchDataActionEffect(IWeatherForecastService weatherForecastService)
    {
        WeatherForecastService = weatherForecastService;
    }

    public override async Task HandleAsync(FetchDataAction action, IDispatcher dispatcher)
    {
        var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    }
}