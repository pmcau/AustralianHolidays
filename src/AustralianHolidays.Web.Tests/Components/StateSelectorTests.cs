public class StateSelectorTests : BunitTestContext
{
    [Test]
    public async Task InitialRender_HasAllButton()
    {
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>()));

        var allButton = cut.Find(".state-btn");
        await Assert.That(allButton.TextContent).IsEqualTo("All");
    }

    [Test]
    public async Task InitialRender_HasAllStateButtons()
    {
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>()));

        var buttons = cut.FindAll(".state-btn");

        // All button + 8 state buttons
        await Assert.That(buttons.Count).IsEqualTo(9);

        var buttonTexts = buttons.Select(b => b.TextContent).ToList();
        await Assert.That(buttonTexts).Contains("All");
        await Assert.That(buttonTexts).Contains("ACT");
        await Assert.That(buttonTexts).Contains("NSW");
        await Assert.That(buttonTexts).Contains("NT");
        await Assert.That(buttonTexts).Contains("QLD");
        await Assert.That(buttonTexts).Contains("SA");
        await Assert.That(buttonTexts).Contains("TAS");
        await Assert.That(buttonTexts).Contains("VIC");
        await Assert.That(buttonTexts).Contains("WA");
    }

    [Test]
    public async Task SelectedStates_ShowsSelectedClass()
    {
        var selectedStates = new HashSet<State> {State.NSW, State.VIC};
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, selectedStates));

        var nswButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "NSW");
        var vicButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "VIC");
        var qldButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "QLD");

        await Assert.That(nswButton.ClassList).Contains("selected");
        await Assert.That(vicButton.ClassList).Contains("selected");
        await Assert.That(qldButton.ClassList).DoesNotContain("selected");
    }

    [Test]
    public async Task AllStatesSelected_AllButtonShowsSelected()
    {
        var allStates = new HashSet<State>(Enum.GetValues<State>());
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, allStates));

        var allButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "All");

        await Assert.That(allButton.ClassList).Contains("selected");
    }

    [Test]
    public async Task ClickStateButton_TogglesSelection()
    {
        IReadOnlySet<State>? selectedStates = null;
        var cut = Render<StateSelector>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>())
            .Add(_ => _.SelectedStatesChanged, (IReadOnlySet<State> s) => selectedStates = s));

        var nswButton = cut.FindAll(".state-btn").First(_ => _.TextContent == "NSW");
        await nswButton.ClickAsync(new());

        await Assert.That(selectedStates).IsNotNull();
        await Assert.That(selectedStates).Contains(State.NSW);
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

        await Assert.That(selectedStates).IsNotNull();
        await Assert.That(selectedStates!.Count).IsEqualTo(8);
    }
}
