using Microsoft.JSInterop;

namespace AustralianHolidays.Web.Services;

public class StatePreferenceService(IJSRuntime jsRuntime)
{
    const string StateKey = "selectedState";
    const string PromptedKey = "locationPrompted";

    public async Task<State?> GetSavedStateAsync()
    {
        var value = await jsRuntime.InvokeAsync<string?>("statePreference.get", StateKey);
        if (value is not null && Enum.TryParse<State>(value, out var state))
        {
            return state;
        }

        return null;
    }

    public async Task SaveStateAsync(State? state)
    {
        if (state.HasValue)
        {
            await jsRuntime.InvokeVoidAsync("statePreference.set", StateKey, state.Value.ToString());
        }
        else
        {
            await jsRuntime.InvokeVoidAsync("statePreference.remove", StateKey);
        }
    }

    public async Task<bool> HasBeenPromptedAsync()
    {
        var value = await jsRuntime.InvokeAsync<string?>("statePreference.get", PromptedKey);
        return value == "true";
    }

    public async Task SetPromptedAsync() =>
        await jsRuntime.InvokeVoidAsync("statePreference.set", PromptedKey, "true");
}
