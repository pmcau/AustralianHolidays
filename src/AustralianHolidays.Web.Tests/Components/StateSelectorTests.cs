using AustralianHolidays.Web.Components;
using Microsoft.AspNetCore.Components;

namespace AustralianHolidays.Web.Tests.Components;

[TestFixture]
public class StateSelectorTests : BunitTestContext
{
    [Test]
    public void InitialRender_HasAllStatesOption()
    {
        var cut = Render<StateSelector>(parameters => parameters
            .Add(p => p.SelectedState, null));

        var options = cut.FindAll("option");

        // All States + 8 individual states
        That(options.Count, Is.EqualTo(9));

        var allStatesOption = options.First(o => o.TextContent == "All States");
        That(allStatesOption, Is.Not.Null);
    }

    [Test]
    public void InitialRender_HasAllIndividualStates()
    {
        var cut = Render<StateSelector>(parameters => parameters
            .Add(p => p.SelectedState, null));

        var options = cut.FindAll("option");
        var optionTexts = options.Select(o => o.TextContent).ToList();

        That(optionTexts, Does.Contain("ACT - Australian Capital Territory"));
        That(optionTexts, Does.Contain("NSW - New South Wales"));
        That(optionTexts, Does.Contain("NT - Northern Territory"));
        That(optionTexts, Does.Contain("QLD - Queensland"));
        That(optionTexts, Does.Contain("SA - South Australia"));
        That(optionTexts, Does.Contain("TAS - Tasmania"));
        That(optionTexts, Does.Contain("VIC - Victoria"));
        That(optionTexts, Does.Contain("WA - Western Australia"));
    }

    [Test]
    public async Task SelectState_TriggersCallback()
    {
        State? selectedState = null;
        var cut = Render<StateSelector>(parameters => parameters
            .Add(p => p.SelectedState, null)
            .Add(p => p.SelectedStateChanged, (State? s) => selectedState = s));

        var select = cut.Find("select");
        await select.ChangeAsync(new ChangeEventArgs { Value = "VIC" });

        That(selectedState, Is.EqualTo(State.VIC));
    }

    [Test]
    public async Task SelectAllStates_TriggersCallbackWithNull()
    {
        State? selectedState = State.NSW;
        var cut = Render<StateSelector>(parameters => parameters
            .Add(p => p.SelectedState, State.NSW)
            .Add(p => p.SelectedStateChanged, (State? s) => selectedState = s));

        var select = cut.Find("select");
        await select.ChangeAsync(new ChangeEventArgs { Value = "" });

        That(selectedState, Is.Null);
    }
}
