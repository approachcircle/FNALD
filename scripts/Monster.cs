using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Monster
{
    public abstract string Name { get; protected set; }

    public virtual Direction Direction { get; protected set; } = Direction.Forward;

    public virtual Room Room { get; protected set; } = Room.Stage;

    public int Hostility { get; protected set; } = 300;

    public virtual int Neutrality { get; protected set; } = 500;

    public virtual bool Roll()
    {
        if (new Random().Next(1, Hostility) == Hostility / 2)
        {
            if (Room == Room.Office) { return false; }
            Direction = Direction.Forward;
            return true;
        }
        else if (new Random().Next(1, Neutrality) == Neutrality / 2)
        {
            if (Room == Room.Stage) return false;
            Direction = Direction.Backward;
            return true;
        } else
        {
            return false;
        }
    }

    public virtual void Move(Direction direction)
    {
        List<Room> rooms = Enum.GetValues<Room>().ToList();
        if (direction == Direction.Forward)
        {
            Room = rooms[rooms.IndexOf(Room) + 1];
        }
        else
        {
            Room = rooms[rooms.IndexOf(Room) - 1];
        }
    }
}
