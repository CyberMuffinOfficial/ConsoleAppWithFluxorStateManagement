using ConsoleAppWithFluxorStateManagement.Store;
using ConsoleAppWithFluxorStateManagement.Store.CounterUseCase;
using ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement;

public class App
{
    private readonly IStore Store;
    public readonly IDispatcher Dispatcher;
    public readonly IState<CounterState> CounterState;
    private readonly IState<WeatherState> WeatherState;

    public App(IStore store, IDispatcher dispatcher, IState<CounterState> counterState, IState<WeatherState> weatherState)
    {
        Store = store;
        Dispatcher = dispatcher;
        CounterState = counterState;
        CounterState.StateChanged += CounterState_StateChanged;
        WeatherState = weatherState;
        WeatherState.StateChanged += WeatherState_StateChanged;
    }

    private void CounterState_StateChanged(object sender, EventArgs e)
    {
        Console.WriteLine("");
        Console.WriteLine("==========================> CounterState");
        Console.WriteLine("ClickCount is " + CounterState.Value.ClickCount);
        Console.WriteLine("<========================== CounterState");
        Console.WriteLine("");
    }

    private void WeatherState_StateChanged(object sender, EventArgs e)
    {
        Console.WriteLine("");
        Console.WriteLine("=========================> WeatherState");
        Console.WriteLine("IsLoading: " + WeatherState.Value.IsLoading);
        if (!WeatherState.Value.Forecasts.Any())
        {
            Console.WriteLine("--- No weather forecasts");
        }
        else
        {
            Console.WriteLine("Temp C\tTemp F\tSummary");
            foreach (WeatherForecast forecast in WeatherState.Value.Forecasts)
                Console.WriteLine($"{forecast.TemperatureC}\t{forecast.TemperatureF}\t{forecast.Summary}");
        }
        Console.WriteLine("<========================== WeatherState");
        Console.WriteLine("");
    }

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("Initializing store");
        Store.InitializeAsync().Wait();
        string input = "";
        do
        {
            Console.WriteLine("1: Increment counter");
            Console.WriteLine("2: Fetch data");
            Console.WriteLine("x: Exit");
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input.ToLowerInvariant())
            {
                case "1":
                    var action = new IncrementCounterAction();
                    Dispatcher.Dispatch(action);
                    break;

                case "2":
                    var fetchDataAction = new FetchDataAction();
                    Dispatcher.Dispatch(fetchDataAction);
                    break;

                case "x":
                    Console.WriteLine("Program terminated");
                    return;
            }

        } while (true);
    }
}
