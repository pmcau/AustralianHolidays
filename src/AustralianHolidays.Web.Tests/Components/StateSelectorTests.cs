[TestFixture]
public class StateSelectorTests : BunitTestContext
{
    [Test]
    public void InitialRender_HasAllButton()
    {
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>()));

        var allButton = cut.Find(".state-btn");
        That(allButton.TextContent, Is.EqualTo("All"));
    }

    [Test]
    public void InitialRender_HasAllStateButtons()
    {
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>()));

        var buttons = cut.FindAll(".state-btn");

        // All button + 8 state buttons
        That(buttons.Count, Is.EqualTo(9));

        var buttonTexts = buttons.Select(b => b.TextContent).ToList();
        That(buttonTexts, Does.Contain("All"));
        That(buttonTexts, Does.Contain("ACT"));
        That(buttonTexts, Does.Contain("NSW"));
        That(buttonTexts, Does.Contain("NT"));
        That(buttonTexts, Does.Contain("QLD"));
        That(buttonTexts, Does.Contain("SA"));
        That(buttonTexts, Does.Contain("TAS"));
        That(buttonTexts, Does.Contain("VIC"));
        That(buttonTexts, Does.Contain("WA"));
    }

    [Test]
    public void SelectedStates_ShowsSelectedClass()
    {
        var selectedStates = new HashSet<State> {State.NSW, State.VIC};
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, selectedStates));

        var nswButton = cut.FindAll(".state-btn").First(b => b.TextContent == "NSW");
        var vicButton = cut.FindAll(".state-btn").First(b => b.TextContent == "VIC");
        var qldButton = cut.FindAll(".state-btn").First(b => b.TextContent == "QLD");

        That(nswButton.ClassList, Does.Contain("selected"));
        That(vicButton.ClassList, Does.Contain("selected"));
        That(qldButton.ClassList, Does.Not.Contain("selected"));
    }

    [Test]
    public void AllStatesSelected_AllButtonShowsSelected()
    {
        var allStates = new HashSet<State>(Enum.GetValues<State>());
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, allStates));

        var allButton = cut.FindAll(".state-btn").First(b => b.TextContent == "All");

        That(allButton.ClassList, Does.Contain("selected"));
    }

    [Test]
    public async Task ClickStateButton_TogglesSelection()
    {
        IReadOnlySet<State>? selectedStates = null;
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>())
            .Add(_ => _.SelectedStatesChanged, (IReadOnlySet<State> s) => selectedStates = s));

        var nswButton = cut.FindAll(".state-btn").First(b => b.TextContent == "NSW");
        await nswButton.ClickAsync(new());

        That(selectedStates, Is.Not.Null);
        That(selectedStates, Does.Contain(State.NSW));
    }

    [Test]
    public async Task ClickAllButton_SelectsAllStates()
    {
        IReadOnlySet<State>? selectedStates = null;
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>())
            .Add(_ => _.SelectedStatesChanged, s => selectedStates = s));

        var allButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "All");
        await allButton.ClickAsync(new());

        That(selectedStates, Is.Not.Null);
        That(selectedStates!.Count, Is.EqualTo(8));
    }
}
