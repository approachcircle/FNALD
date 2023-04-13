public class Harry : Monster
{
	public override string Name { get; protected set; } = "Harry";

    public override void Move(Direction direction)
    {
        if (direction is Direction.Forward)
        {
            if (Room is Room.A2)
                Goto(Room.Left);
            else
                Advance();
        }
        else
        {
            if (Room is Room.Left)
                Goto(Room.A2);
            else
                Regress();
        }
    }
}

