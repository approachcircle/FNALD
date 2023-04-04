using Godot;
using System;

public partial class Jumpscare : Node2D
{
	public override void _Ready()
	{
		if (Global.RemainingTime <= 0.1 && Global.Time == 5)
		{
			EndNight();
		}
		else if (Global.Time == 5)
		{
			GetNode<Timer>("Remaining").Start(Global.RemainingTime);
		}
		int pause = new Random().Next(5, 12);
		GetNode<Timer>("Pause").Timeout += Trigger;
		GetNode<Timer>("Pause").Start(pause);
		GetNode<Timer>("Remaining").Timeout += EndNight;
	}

	public override void _Process(double delta)
	{
	}

	private async void Trigger()
	{
		GetNode<Sprite2D>("Logan").Visible = true;
		GetNode<AudioStreamPlayer>("Scream").Play();
		await ToSignal(GetNode<AudioStreamPlayer>("Scream"), "finished");
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void EndNight()
	{
		GetTree().ChangeSceneToFile("res://scenes/night_end.tscn");
	}
}
