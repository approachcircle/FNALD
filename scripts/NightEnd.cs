using Godot;
using System;

public partial class NightEnd : Control
{
	bool animating = false;

	[Export]
	int speed = 25;

	public override void _Ready()
	{
		GetNode<Timer>("Pause").Timeout += PauseEnd;
		GetNode<AudioStreamPlayer>("Chime").Finished += AudioFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (animating)
		{
			GetNode<RigidBody2D>("Hours").MoveAndCollide(new Vector2(0, (float)(speed * delta)));
		}
	}

	private void PauseEnd() => animating = true;

	private void AudioFinished()
	{
		Global.SetNight(Global.GetNight() + 1);
		GetTree().ChangeSceneToFile("res://scenes/transition.tscn");
	}
}
