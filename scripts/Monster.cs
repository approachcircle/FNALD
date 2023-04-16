using System;

public abstract class Monster : IEquatable<Monster>
{
    public abstract string Name { get; protected set; }

    public virtual Direction Direction { get; protected set; }

    public virtual Room Room { get; protected set; } = Room.Stage;

    protected virtual int Night { get; set; }

    public virtual int HostilityMultiplier { get; protected set; } = 53;

    public virtual int NeutralityMultiplier { get; protected set; } = 5;

    public virtual int BaseHostility { get; protected set; } = 300;

    public virtual int DynamicHostility { get; protected set; }

    public virtual int BaseNeutrality { get; protected set; } = 115;

    public virtual int DynamicNeutrality { get; protected set; }

    public abstract Room PeekLocation { get; protected set; }

    public Monster()
    {
        CalculateDifficulty();
        Night = (int)Global.GetNight();
    }

    public virtual void Roll()
    {
        if (Global.AngerRate != 1) DynamicHostility -= Global.AngerRate * 2;
        if (Global.AngerRate != 1) DynamicNeutrality += Global.AngerRate * 2;
        if (new Random().Next(1, DynamicHostility) == DynamicHostility / 2)
        {
            if (Room != Room.Office)
                Direction = Direction.Forward;
        }
        else if (new Random().Next(1, DynamicNeutrality) == DynamicNeutrality / 2)
        {
            if (Room != Room.Stage && Room != Room.B1)
                Direction = Direction.Backward;
        }
        else
        {
            CalculateDifficulty();
            Direction = Direction.Static;
        }
    }

    public virtual void Move()
    {
        if (Direction is Direction.Forward)
        {
            if (Room is Room.A2)
                Goto(PeekLocation);
            else if (Room == PeekLocation)
                Goto(Room.Office);
            else
                Advance();
        }
        else if (Direction is Direction.Backward)
        {
            if (Room == PeekLocation)
                Goto(Room.B2);
            else
                Regress();
        }
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
        DynamicHostility = BaseHostility - (HostilityMultiplier * Night);
        DynamicNeutrality = BaseNeutrality + (NeutralityMultiplier * Night);
    }
}
