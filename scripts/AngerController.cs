using Godot;
using System;

public partial class AngerController : Node
{
	public override void _Ready()
	{
		GetNode<AngerMeter>("../HUD/AngerMeter").Angry += TriggerAnger;
	}

	public override void _Process(double delta)
	{
	}

	private async void TriggerAnger()
	{
		AnimationPlayer animator = GetNode<AnimationPlayer>("../HUD/AngerAnimation");
		Global.PlayerDisabled = true;
		animator.Play("move_meter");
		await ToSignal(animator, "animation_finished");
		animator.Play("fade_in");
		GetNode<AudioStreamPlayer>("../AngrySound").Play();
		await ToSignal(GetNode<AudioStreamPlayer>("../AngrySound"), "finished");
		GetTree().ChangeSceneToFile("res://scenes/jumpscare.tscn");
	}
}
