using OneOf;

namespace Core;

public class DieResult(OneOf<Fruit, SpecialEvent> input) : OneOfBase<Fruit, SpecialEvent>(input);
