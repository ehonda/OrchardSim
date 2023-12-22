using System.Globalization;
using System.Text;
using App;
using Core;
using Spectre.Console;
using Spectre.Console.Rendering;

// Make Emoji work in PowerShell, see: https://github.com/spectreconsole/spectre.console/issues/113
Console.OutputEncoding = Encoding.UTF8;

const int simulationCount = 10_000;
const int maxFruitCount = 4;
const int maxRavenFieldCount = 5;

var table = new Table
{
    Border = TableBorder.Rounded,
};

table
    .AddColumn("🦜 / 🍏")
    .AddColumns(Enumerable
        .Range(1, maxFruitCount)
        .Select(i => i.ToString(CultureInfo.InvariantCulture))
        .ToArray());

for (var ravenFieldCount = 1; ravenFieldCount <= maxRavenFieldCount; ravenFieldCount++)
{
    var cells = Enumerable
        .Range(1, maxFruitCount)
        .Select(fruitCount => RunSimulation(simulationCount, fruitCount, ravenFieldCount))
        .Prepend(new Text($"{ravenFieldCount.ToString(CultureInfo.InvariantCulture)}"))
        .ToArray();

    table.AddRow(cells);
}

AnsiConsole.WriteLine($"Simulation count: {simulationCount}");
AnsiConsole.WriteLine();
AnsiConsole.Write(table);

static IRenderable RunSimulation(int simulationCount, int fruitCount, int ravenFieldCount)
{
    var results = Enumerable
        .Range(0, simulationCount)
        .Select(_ => new OrchardGame(fruitCount, ravenFieldCount).Play())
        .GroupBy(result => result)
        .ToDictionary(
            grouping => grouping.Key,
            grouping => new GameResultSummary(grouping.Count(), grouping.Count() / (double)simulationCount));

    var rows = new Rows(
        new Text($"Fruit count: {fruitCount}"),
        new Text($"Raven field count: {ravenFieldCount}"),
        new Text(""),
        new Markup($"[green]Victories[/]: {SummaryString(results[GameResult.Victory])}"),
        new Markup($"[red]Defeats[/]: {SummaryString(results[GameResult.Defeat])}"));
    
    var panel = new Panel(rows)
    {
        Border = BoxBorder.Rounded
    };

    return panel;
}

static string SummaryString(GameResultSummary summary)
{
    var absoluteCountString = summary.AbsoluteCount.ToString(CultureInfo.InvariantCulture);
    var percentageString = summary.RelativeCount.ToString("P", CultureInfo.InvariantCulture);
    return $"{absoluteCountString} ≈ {percentageString}";
}
