using Godot;
using System;

public partial class DistractionController : Node
{
	Button skipButton;
	Sprite2D dist;
	Timer roller;
	int chance = 75;

	[Signal]
	public delegate void DistractedEventHandler();
	public override void _Ready()
	{
		skipButton = GetNode<Button>("SkipButton");
		dist = GetNode<Sprite2D>("Distraction");
		roller = GetNode<Timer>("Roller");
		skipButton.Pressed += StopDistract;
		roller.Timeout += Roll;
		GetNode<Button>("DistractButton").Pressed += Distract;
	}

	public override void _Process(double delta)
	{
		if (Global.PlayerDisabled)
			StopDistract();
	}

	private void Roll()
	{
		int result = new Random().Next(0, chance);
		if (result == chance / 2)
		{
			Distract();
		}
	}

	private void Distract()
	{
		if (!Global.Distracted && !Global.PlayerDisabled && !Global.PhoneActive)
		{
			EmitSignal(nameof(Distracted));
			Global.Distracted = true;
			Global.CamerasEnabled = false;
			int distNumber = new Random().Next(1, 6);
			string path = $"res://assets/dist-{distNumber}.png";
			dist.Texture = GD.Load<CompressedTexture2D>(path);
			dist.Visible = true;
			GetNode<Label>("Skip").Visible = true;
			skipButton.Visible = true;
			Global.AngerRate++;
			GetNode<AudioStreamPlayer>("DistractionSound").Play();
		}
	}

	private void StopDistract()
	{
		if (Global.Distracted)
		{
			Global.CamerasEnabled = true;
			dist.Visible = false;
			GetNode<Label>("Skip").Visible = false;
			skipButton.Visible = false;
			Global.Distracted = false;
			Global.AngerRate--;
			GetNode<AudioStreamPlayer>("DistractionSound").Stop();
		}
	}
}
