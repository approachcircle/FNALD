using Godot;

public partial class MonsterController : Node
{
	[Signal]
	public delegate void MonsterMovedEventHandler(string monsterName);

	private static readonly Monster[] monsters = {
		new Cameron(),
		new Harry()
	};

	public override void _Ready()
	{
		GetNode<Timer>("Movement").Timeout += RollMonsters;
		MonsterMoved += (monsterName) => { GetNode<Label>("MoveList").Text += $"\n{monsterName}"; };
	}

	public override void _Process(double delta)
	{

	}

	private void RollMonsters()
	{
		if (!Global.MonstersEnabled) return;
		foreach (Monster monster in monsters)
		{
			if (monster.Roll())
			{
				EmitSignal(nameof(MonsterMoved), monster.Name);
				monster.Move(monster.Direction);
			}
		}
	}
}
