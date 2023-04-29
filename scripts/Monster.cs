﻿using System;

public abstract class Monster : IEquatable<Monster>
{
    public abstract string Name { get; }

    public virtual Direction Direction { get; protected set; }

    public virtual Room Room { get; protected set; } = Room.Stage;

    protected virtual int Night { get; set; }

    public virtual int HostilityMultiplier { get; } = 53;

    public virtual int NeutralityMultiplier { get; } = 5;

    public virtual int BaseHostility { get; } = 325;

    public virtual int DynamicHostility { get; protected set; }

    public virtual int BaseNeutrality { get; } = 113;

    public virtual int DynamicNeutrality { get; protected set; }

    public abstract Room PeekLocation { get; }

    public virtual MonsterState State { get; set; } = MonsterState.Idle;

    public virtual bool SuppressJumpscare { get; } = false;

    public virtual bool SuppressStateChange { get; } = false;

    public virtual bool SuppressPeeking { get; } = false;

    public virtual bool IsPeeking {
        get
        {
            return Room == PeekLocation;
        }
    }

    public virtual bool CanAttack {
        get {
            return CanEnterOffice() && IsPeeking;
        }
    }

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
            else if (IsPeeking)
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
            if (IsPeeking)
                Goto(Room.B2);
            else
                Regress();
        }
    }

    public bool Equals(Monster monster)
    {
        return Name == monster.Name;
    }

    public virtual void Advance()
    {
        var rooms = Room.GetRooms();
        Room = rooms[rooms.IndexOf(Room) + 1];
    }
    public virtual void Regress()
    {
        var rooms = Room.GetRooms();
        Room = rooms[rooms.IndexOf(Room) - 1];
    }

    public void Goto(Room room)
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
        if (IsPeeking && !CanEnterOffice()) DynamicNeutrality /= 2;
    }
}
