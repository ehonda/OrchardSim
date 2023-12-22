namespace Core;

public class Die
{
    private readonly Random _random = new();

    private readonly DieResult[] _choices = Enum
        .GetValues<Fruit>()
        .Select(fruit => new DieResult(fruit))
        .Concat(Enum
            .GetValues<SpecialEvent>()
            .Select(specialEvent => new DieResult(specialEvent)))
        .ToArray();

    public DieResult Roll() => _choices[_random.Next(_choices.Length)];
}
