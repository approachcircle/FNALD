using System;
using System.Collections.Generic;
using System.Linq;

public enum Room
{
    Stage,
    B1,
    B2,
    A1,
    A2,
    C1,
    C2,
    Office
}
public static class Extensions
{
    public static List<Room> GetRooms(this Room room)
    {
        return Enum.GetValues<Room>().ToList();
    }
}
