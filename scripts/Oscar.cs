using Godot;

public class Oscar : Monster
{
    public override string Name => "Oscar";
    public override Room PeekLocation => Room.Office;
    public override bool SuppressStateChange => true;
    public override int HostilityMultiplier => 49;
    public override int NeutralityMultiplier => 50;
    public override bool CanEnterOffice => true;

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