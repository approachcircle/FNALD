using Godot;

public partial class MonsterController : Node
{
	[Signal]
	public delegate void MonsterMovedEventHandler(string monsterName);

	public static Monster[] Monsters { get; } = {
		new Cameron(),
		new Harry()
	};

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
				EmitSignal(nameof(MonsterMoved), monster.Name);
				monster.Move(monster.Direction);
			}
		}
	}
}
