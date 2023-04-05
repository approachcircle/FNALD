using Godot;
using System.Collections.Generic;

public partial class MonsterController : Node
{
    public static Monster[] Monsters { get; set; } = {
		new Cameron()
	};
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		if (Global.MonstersEnabled)
		{
			foreach (Monster monster in Monsters)
				if (monster.Roll())
					// TODO: monster movement behaviour
					monster.Room = monster.Destination;
		}
	}
}
