using ConsoleAppWithFluxorStateManagement.Store;
using ConsoleAppWithFluxorStateManagement.Store.CounterUseCase;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement;

/* 
 * The purpose of this demo is to demonstrate how to use fluxor state management in a dotnet application
 * First we configure App.cs in Program.cs. Add dependency injection and the fluxor service that automatically scans the assembly for annotated classes/methods
 * In App.cs, we create an infinite do while loop, and give the user the ability to increment the count
 * When the command is issued, we dispatch a command with the class name as a parameter that is used as an identifier to determine which reducer function to execute
 * The reducer will be triggered based on the class that is passed in the dispatch command
 * The reducer function/method will create a new instance of the class that holds the state, and will push it to the store.
 * In the reducer function, we need to pass both the name of the state that is to be replaced, as well as the action that gives us the values that we will need to change
 * We must subscribe to this event 
 */
public class App
{
    private readonly IStore Store;
    public readonly IDispatcher Dispatcher;
    public readonly IState<CounterState> CounterState;

    public App(IStore store, IDispatcher dispatcher, IState<CounterState> counterState)
    {
        Store = store;
        Dispatcher = dispatcher;
        CounterState = counterState;
        CounterState.StateChanged += CounterState_StateChanged;
    }

    private void CounterState_StateChanged(object sender, EventArgs e)
    {
        Console.WriteLine("");
        Console.WriteLine("==========================> CounterState");
        Console.WriteLine("ClickCount is " + CounterState.Value.ClickCount);
        Console.WriteLine("<========================== CounterState");
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
            Console.WriteLine("x: Exit");
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input.ToLowerInvariant())
            {
                case "1":
                    var action = new IncrementCounterAction();
                    Dispatcher.Dispatch(action);
                    break;

                case "x":
                    Console.WriteLine("Program terminated");
                    return;
            }

        } while (true);
    }
}
