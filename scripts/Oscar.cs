using Godot;

public class Oscar : Monster
{
    public override string Name { get; } = "Oscar";
    public override Room PeekLocation { get; } = Room.Office;
    public override bool SuppressStateChange { get; } = true;
    public override int HostilityMultiplier { get; } = 49;
    public override int NeutralityMultiplier { get; } = 50;

    public Oscar() : base()
    {
        QuicktimeController.QuicktimeFinished += (_, _) => QuickTimeFinished();
    }

    public override void Roll()
    {
        if (Room is Room.Office)
        {
            QuicktimeController.StartEvent(new Key[] { Key.Q });
        }
        base.Roll();
    }

    public override void Move()
    {
        if (Direction is Direction.Forward)
            Goto(Room.Office);
    }

    private void QuickTimeFinished()
    {
        Goto(Room.Stage);
    }
}