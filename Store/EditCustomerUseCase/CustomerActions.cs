using ConsoleAppWithFluxorStateManagement.Shared.ApiObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithFluxorStateManagement.Store.EditCustomerUseCase;
public class GetCustomerForEditAction
{
    public int Id { get; }

    public GetCustomerForEditAction(int id)
    {
        Id = id;
    }
}

public class GetCustomerForEditResultAction
{
    public CustomerEdit Customer { get; }

    public GetCustomerForEditResultAction(CustomerEdit customer)
    {
        Customer = customer;
    }
}

[FeatureState]
public class EditCustomerState
{
    public bool IsLoading { get; private set; }

    private EditCustomerState() { } // Required for creating initial state
    public EditCustomerState(bool isLoading)
    {
        IsLoading = isLoading;
    }
}

public static class Reducers
{
    [ReducerMethod(typeof(GetCustomerForEditAction))]
    public static EditCustomerState Reduce(EditCustomerState state) =>
      new EditCustomerState(isLoading: true);

    [ReducerMethod(typeof(GetCustomerForEditResultAction))]
    public static EditCustomerState ReduceTwo(EditCustomerState state) =>
      new EditCustomerState(isLoading: false);
}

public class Effects
{
    [EffectMethod(typeof(GetCustomerForEditAction))]
    public async Task HandleGetCustomerForEditAction(IDispatcher dispatcher)
    {
        Console.WriteLine("Getting customer with Id: 42");

        await Task.Delay(1000);

        string jsonFromServer = $"{{\"Id\":42,\"RowVersion\":\"AQIDBAUGBwgJCgsMDQ4PEA==\",\"Name\":\"Our first customer\"}}";
        var objectFromServer = JsonConvert.DeserializeObject<CustomerEdit>(jsonFromServer);
        dispatcher.Dispatch(new GetCustomerForEditResultAction(objectFromServer));
    }
}