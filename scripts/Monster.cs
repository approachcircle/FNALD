using System;

public abstract class Monster : IEquatable<Monster>
{
    public abstract string Name { get; protected set; }

    public virtual Direction Direction { get; protected set; }

    public virtual Room Room { get; protected set; } = Room.Stage;

    protected virtual int Night { get; set; }

    public virtual int HostilityMultiplier { get; protected set; } = 53;

    public virtual int NeutralityMultiplier { get; protected set; } = 5;

    public virtual int BaseHostility { get; protected set; } = 330;

    public virtual int DynamicHostility { get; protected set; }

    public virtual int BaseNeutrality { get; protected set; } = 113;

    public virtual int DynamicNeutrality { get; protected set; }

    public abstract Room PeekLocation { get; protected set; }

    public Monster()
    {
        CalculateDifficulty();
        Night = (int)Global.GetNight();
    }

    public virtual void Roll()
    {
        CalculateDifficulty();
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
                if (CanEnterOffice())
                    Goto(Room.Office);
                else
                    if (PeekLocation == Room.Left)
                        Global.KnockingLeft = true;
                    else
                        Global.KnockingMid = true;
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

    protected void Goto(Room room)
    {
        Room = room;
    }
    
    public bool CanEnterOffice()
    {
        return (PeekLocation == Room.Left && !Global.LeftDoorClosed) ||
            (PeekLocation == Room.Mid && !Global.MidDoorClosed);
    }

    protected void CalculateDifficulty()
    {
        DynamicHostility = BaseHostility - (HostilityMultiplier * Night);
        DynamicNeutrality = BaseNeutrality + (NeutralityMultiplier * Night);
        if (Global.AngerRate != 1) DynamicHostility -= Global.AngerRate;
        if (Global.AngerRate != 1) DynamicNeutrality += Global.AngerRate;
        if (Room == PeekLocation && !CanEnterOffice()) DynamicNeutrality /= 2;
    }
}
