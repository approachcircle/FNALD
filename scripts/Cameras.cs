using Godot;
using System;

public partial class Cameras : Control
{
	bool lightVisible = true;
	public override void _Ready()
	{
		GetNode<Timer>("Flash").Timeout += () => { lightVisible = !lightVisible; };
	}

	public override void _Process(double delta)
	{
		GetNode<Label>("DisabledWarning").Visible = !Global.MonstersEnabled;
		string buffer = "Monsters:";
		foreach (Monster monster in MonsterController.Monsters)
			buffer += $"\n{monster.Name}: {monster.Room} (H: {monster.DynamicHostility} N: {monster.DynamicNeutrality} D: {monster.Direction})";
		GetNode<Label>("Rooms").Text = buffer;
		FlashLight();
	}

	private void FlashLight()
	{
		Sprite2D recording = GetNode<Sprite2D>("Recording");
		if (lightVisible)
			Global.ModulateNodeAlpha(recording, 1);
		else
			Global.ModulateNodeAlpha(recording, 0);
	}
}
