using System;

public abstract class Monster : IEquatable<Monster>
{
    public abstract string Name { get; protected set; }

    public virtual Direction Direction { get; protected set; }

    public virtual Room Room { get; protected set; } = Room.Stage;

    protected virtual int Night { get; set; }

    public virtual int BaseHostility { get; protected set; } = 300;

    public virtual int DynamicHostility { get; protected set; }

    public virtual int BaseNeutrality { get; protected set; } = 115;

    public virtual int DynamicNeutrality { get; protected set; }

    public Monster()
    {
        CalculateDifficulty();
        Night = (int)Global.GetNight();
    }

    public virtual bool Roll()
    {
        if (Global.AngerRate != 1) DynamicHostility -= Global.AngerRate * 2;
        if (Global.AngerRate != 1) DynamicNeutrality += Global.AngerRate * 2;
        if (new Random().Next(1, DynamicHostility) == DynamicHostility / 2)
        {
            if (Room == Room.Office) return false;
            Direction = Direction.Forward;
            return true;
        }
        else if (new Random().Next(1, DynamicNeutrality) == DynamicNeutrality / 2)
        {
            if (Room == Room.Stage) return false;
            if (Room == Room.B1) return false;
            Direction = Direction.Backward;
            return true;
        }
        else
        {
            CalculateDifficulty();
            return false;
        }
    }

    public virtual void Move(Direction direction)
    {
        if (direction is Direction.Forward)
            Advance();
        else
            Regress();
    }

    public bool Equals(Monster monster)
    {
        return Name == monster.Name;
    }

    protected virtual void Advance()
    {
        var rooms = Room.GetRooms();
        Room = rooms[rooms.IndexOf(Room) + 1];
    }
    protected virtual void Regress()
    {
        var rooms = Room.GetRooms();
        Room = rooms[rooms.IndexOf(Room) - 1];
    }

    protected virtual void Goto(Room room)
    {
        Room = room;
    }

    protected virtual void CalculateDifficulty()
    {
        DynamicHostility = BaseHostility - (53 * Night);
        DynamicNeutrality = BaseNeutrality + (5 * Night);
    }
}
