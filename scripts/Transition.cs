using Godot;
using System;
using System.Threading;

public partial class Transition : Control
{
	PackedScene office;
	Thread thread;

	public async override void _Ready()
	{
		thread = new Thread(() => {
			try
			{
				office = GD.Load<PackedScene>("res://scenes/office.tscn");
			}
			finally
			{
				BeginNight();
			}
		});
		GetNode<Godot.Timer>("Timer").Timeout += () => { GetNode<Label>("Warning").Visible = true; };
		Global.Time = 12;
		GetNode<Label>("Night").Text = $"Night {Global.GetNight()}";
		GetNode<AnimatedSprite2D>("Wait").Play();
		GetNode<AnimationPlayer>("Animator").Play("uncover");
		await ToSignal(GetNode<AnimationPlayer>("Animator"), "animation_finished");
		thread.Start();
	}

	private void BeginNight() => GetTree().ChangeSceneToPacked(office);
}
