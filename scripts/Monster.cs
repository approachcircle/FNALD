using Godot;

public abstract class Monster
{
/*    [Signal]
    public delegate void MonsterMovedEventHandler();*/

    public string Name { get; set; } = "Monster";


    public Room Room { get; set; } = Room.Stage;

    public int Hostility { get; set; } = 0;
    public Room Destination { get; set; } = Room.Stage;

    public abstract bool Roll();

    public abstract void SetDestination();
}
