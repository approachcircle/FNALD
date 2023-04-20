using Godot;
using System;

public partial class Jumpscare : Node2D
{
	Sprite2D image;
	public override void _Ready()
	{
		image = GetNode<Sprite2D>("Image");
		if (Global.AttackType is AttackType.Angry)
		{
			image.Texture = GD.Load<CompressedTexture2D>("res://assets/LoganJump.jpg");
			InitAngryAttack();
		}
		else if (Global.AttackType is AttackType.Monster)
		{
			image.Texture = GD.Load<CompressedTexture2D>($"res://assets/{MonsterController.AttackingMonster.Name}Jump");
			TriggerAttack();
		}
	}

	public override void _Process(double delta)
	{
	}

	private void InitAngryAttack()
	{
		if (Global.RemainingTime <= 0.1 && Global.Time == 5)
		{
			EndNight();
		}
		else if (Global.Time == 5)
		{
			GetNode<Timer>("Remaining").Start(Global.RemainingTime);
		}
		int pause = new Random().Next(7, 15);
		GetNode<Timer>("Pause").Timeout += TriggerAttack;
		GetNode<Timer>("Pause").Start(pause);
		GetNode<Timer>("Remaining").Timeout += EndNight;
	}

	private async void TriggerAttack()
	{
		GetNode<Sprite2D>("Image").Visible = true;
		GetNode<AudioStreamPlayer>("Scream").Play();
		await ToSignal(GetNode<AudioStreamPlayer>("Scream"), "finished");
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void EndNight()
	{
		GetTree().ChangeSceneToFile("res://scenes/night_end.tscn");
	}
}
