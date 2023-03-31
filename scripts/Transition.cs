using Godot;
using System;

public partial class Transition : Control
{
	bool uncovering = false;
	int speed = 250;

	public override void _Ready()
	{
		Global.Time = 12;
		GetNode<Label>("Night").Text = $"Night {Global.GetNight()}";
		GetNode<AnimatedSprite2D>("Wait").Play();
		GetNode<Timer>("Timer").Timeout += BeginNight;
		uncovering = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetNode<CharacterBody2D>("Cover").MoveAndCollide(new Vector2((float)(speed * delta), 0));
	}

	private void BeginNight() => GetTree().ChangeSceneToFile("res://scenes/office.tscn");
}
