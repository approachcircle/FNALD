public class Millie : Monster
{
    public override string Name { get; protected set; } = "Millie";
    public override Room PeekLocation { get; protected set; } = Room.Mid;
    public override int HostilityMultiplier { get; protected set; } = 55;
}
