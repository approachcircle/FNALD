using Godot;
using System.Collections.Generic;

public partial class MonsterController : Node
{
	[Signal]
	public delegate void MonsterMovedEventHandler(string monsterName);

	public static Monster[] Monsters { get; } = {
		new Cameron(),
		new Harry()
	};

	public static List<Monster> PeekingLeft { get; } = new List<Monster>();
	public static List<Monster> LastLeft { get; } = new List<Monster>();
    public static List<Monster> PeekingMid { get; } = new List<Monster>();
    public static List<Monster> LastMid { get; } = new List<Monster>();

    public override void _Ready()
	{
		GetNode<Timer>("Movement").Timeout += RollMonsters;
	}

	public override void _Process(double delta)
	{

	}

	private void RollMonsters()
	{
		if (!Global.MonstersEnabled) return;
		foreach (Monster monster in Monsters)
		{
			if (monster.Roll())
			{
				monster.Move(monster.Direction);
				EmitSignal(nameof(MonsterMoved), monster.Name);
				CheckPeeking(monster);
			}
		}
	}

	private void CheckPeeking(Monster monster)
	{
		if (monster.Room is not Room.Mid)
		{
            if (PeekingMid.Contains(monster))
			{
				PeekingMid.Remove(monster);
			}
        }
		if (monster.Room is not Room.Left)
		{
			if (PeekingLeft.Contains(monster))
			{
                PeekingLeft.Remove(monster);
			}
		}
		if (monster.Room is Room.Mid)
		{
			if (!PeekingMid.Contains(monster))
			{
                PeekingMid.Add(monster);
				LastMid.Add(monster);
            }
		}
		if (monster.Room is Room.Left)
		{
            if (!PeekingLeft.Contains(monster))
			{
                PeekingLeft.Add(monster);
				LastLeft.Add(monster);
            }
		}
	}
}
