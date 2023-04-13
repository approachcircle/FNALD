public class Cameron : Monster
{
    public override string Name { get; protected set; } = "Cameron";

    public override void Move(Direction direction)
    {
        if (direction is Direction.Forward)
        {
            if (Room is Room.A2)
                Goto(Room.Mid);
            else if (Room is Room.Mid)
                Goto(Room.Office);
            else
                Advance();
        }
        else
            if (Room is Room.Office)
                Goto(Room.B2);
            else
                Regress();
    }
}