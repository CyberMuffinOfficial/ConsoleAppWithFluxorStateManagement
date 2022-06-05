using ConsoleAppWithFluxorStateManagement.Store;
using ConsoleAppWithFluxorStateManagement.Store.CounterUseCase;
using ConsoleAppWithFluxorStateManagement.Store.EditCustomerUseCase;
using ConsoleAppWithFluxorStateManagement.Store.WeatherUseCase;
using Fluxor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement;

/*
 Our App class dispatches the FetchDataAction action to notify the store of our intent.
The ReduceFetchDataAction reducer method sets IsLoading to true, so our UI can reflect the change.
The effect method is triggered by the FetchDataAction and asynchronously makes a data request to our mock server.
The call to Dispatcher.Dispatch(fetchDataAction) in our App class completes, so redisplays the menu options.
One second later the mock service returns data.
The effect method bundles the result data into a new FetchDataResultAction and dispatches the action.
The ReduceFetchDataResultAction reducer methods sets IsLoading to false, and sets Forecasts to the values in the action.
The store now has new state for WeatherState so executes its StateChanged event, resulting in the new state being output to the console below the options menu.

 
 Middleware lifecycle
Task InitializeAsync(IStore store)
Executed when the store is first initialised. This gives us an opportunity to store away a reference to the store that has been initialized.
void AfterInitializeAllMiddlewares()
Once the store has been initialised, and InitializeAsync has been executed on all Middleware, this method will be executed.
bool MayDispatchAction(object action)
Every time IDispatcher.Dispatch is executed, the action dispatched will first be passed to every Middleware in turn to give it the chance to veto the action. If the method returns true then the action will be dispatched. The first Middleware to return false will terminate the dispatch process. An example of this is the ReduxDevToolsMiddleware.cs class which prevents the dispatching of new actions when the user is viewing a historical state.
void BeforeDispatch(object action)
Once all Middlewares have approved, this method is called to inform us the action is about to be reduced into state.
void AfterDispatch(object action)
After the action has been processed by all reducers this method will be called.

 IActionSubscriber allows us to subscribe to the dispatch pipeline and be notifified whenever an action has been dispatched.

One particularly useful example of this is when we wish to retrieve a mutable object from an API server that we can edit in our application without having to store that mutable object in our immutable state.

Another is when an action such as CustomerSearchAction is dispatched and the UI needs to know when it is complete so it can set focus to a specific control.


This tutorial will demonstrate how to subscribe to be notified whenever a specific action is dispatched. When the subscriber is notified, a JSON representation of the action will be displayed in the console.

 */
public class App : IDisposable
{
    private readonly IStore Store;
    public readonly IDispatcher Dispatcher;
    private readonly IActionSubscriber ActionSubscriber;

    public App(IStore store, IDispatcher dispatcher, IActionSubscriber actionSubscriber)
    {
        Store = store;
        Dispatcher = dispatcher;
        ActionSubscriber = actionSubscriber;
    }

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("Initializing store");
        Store.InitializeAsync().Wait();
        SubscribeToResultAction();
        string input = "";
        do
        {
            Console.WriteLine("1: Get mutable object from API server");
            Console.WriteLine("x: Exit");
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input.ToLowerInvariant())
            {
                case "1":
                    var getCustomerAction = new GetCustomerForEditAction(42);
                    Dispatcher.Dispatch(getCustomerAction);
                    break;

                case "x":
                    Console.WriteLine("Program terminated");
                    return;
            }
        } while (true);
    }

    private void SubscribeToResultAction()
    {
        Console.WriteLine($"Subscribing to action {nameof(GetCustomerForEditResultAction)}");
        ActionSubscriber.SubscribeToAction<GetCustomerForEditResultAction>(this, action =>
        {
            // Show the object from the server in the console
            string jsonToShowInConsole = JsonConvert.SerializeObject(action.Customer, Formatting.Indented);
            Console.WriteLine("Action notification: " + action.GetType().Name);
            Console.WriteLine(jsonToShowInConsole);
        });
    }

    void IDisposable.Dispose()
    {
        // IMPORTANT: Unsubscribe to avoid memory leaks!
        ActionSubscriber.UnsubscribeFromAllActions(this);
    }
}