public class Millie : Monster
{
    public override string Name { get; } = "Millie";
    public override Room PeekLocation { get; } = Room.Mid;
    public override int HostilityMultiplier { get; } = 55;
}
