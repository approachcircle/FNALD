using Godot;
using System;

public partial class Victory : Control
{
	RigidBody2D credits;
	RigidBody2D victoryText;
	Timer pause;
	bool scroll = false;
	float speed = 100;
	public override void _Ready()
	{
		credits = GetNode<RigidBody2D>("Credits");
		victoryText = GetNode<RigidBody2D>("VictoryText");
		pause = GetNode<Timer>("Pause");
		pause.Timeout += () => { scroll = true; };
		Global.SetNight(1);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("next_menu"))
			GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (scroll)
		{
			victoryText.MoveAndCollide(new Vector2(0, -speed * (float)delta));
			var collided = credits.MoveAndCollide(new Vector2(0, -speed * (float)delta));
			if (collided.GetCollider() is RigidBody2D)
				GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
		}
	}
}
