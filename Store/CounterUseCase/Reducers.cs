using ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement.Store.CounterUseCase;

public static class Reducers
{
    [ReducerMethod(typeof(IncrementCounterAction))]
    public static CounterState ReduceIncrementCounterAction(CounterState state) =>
      new CounterState(clickCount: state.ClickCount + 1);

    //[ReducerMethod]
    //public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action) =>
    //    new WeatherState(isLoading: true, forecasts: null);

    [ReducerMethod]
    public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action) =>
      new WeatherState(
        isLoading: true,
        forecasts: null);

    [ReducerMethod]
    public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action) =>
      new WeatherState(
        isLoading: false,
        forecasts: action.Forecasts);

    //Or can do this
    //[ReducerMethod(typeof(FetchDataAction))]
    //public static WeatherState ReduceFetchDataAction(WeatherState state) =>
    //    new WeatherState(
    //    isLoading: true,
    //    forecasts: null);
}
