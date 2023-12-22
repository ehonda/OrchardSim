using System.Diagnostics;

namespace Core;

public class OrchardGame(int fruitCount, int ravenFieldCount)
{
    private readonly Dictionary<Fruit, int> _trees = Enum
        .GetValues<Fruit>()
        .ToDictionary(fruit => fruit, _ => fruitCount);

    private int _ravenPosition;
    
    private readonly Die _die = new();
    
    public GameResult Play()
    {
        while (true)
        {
            var turnResult = Turn();
            switch (turnResult)
            {
                case TurnResult.Victory:
                    return GameResult.Victory;
                
                case TurnResult.Defeat:
                    return GameResult.Defeat;
                
                case TurnResult.Continue:
                    continue;
                
                default:
                    throw new UnreachableException($"Unknown turn result {turnResult}");
            }
        }
    }

    private TurnResult Turn()
    {
        _die.Roll().Switch(
            fruit =>
            {
                if (_trees[fruit] > 0)
                {
                    _trees[fruit]--;
                }
            },
            specialEvent =>
            {
                switch (specialEvent)
                {
                    case SpecialEvent.PickAnyFruit:
                    {
                        var fruitWithHighestCount = _trees.MaxBy(kv => kv.Value).Key;
                        _trees[fruitWithHighestCount]--;
                        return;
                    }

                    case SpecialEvent.MoveRaven:
                        _ravenPosition++;
                        return;

                    default:
                        throw new UnreachableException($"Unknown special event {specialEvent}");
                }
            });

        if (_trees.All(kv => kv.Value == 0))
        {
            return TurnResult.Victory;
        }
        
        return _ravenPosition > ravenFieldCount
            ? TurnResult.Defeat
            : TurnResult.Continue;
    }
}
