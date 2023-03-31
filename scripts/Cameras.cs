using Godot;
using System;

public partial class Cameras : Control
{
	bool light_visible = true;
	public override void _Ready()
	{
		GetNode<Timer>("Flash").Timeout += Flash;
	}

	public override void _Process(double delta)
	{
		Sprite2D recording = GetNode<Sprite2D>("Recording");
		if (light_visible)
		{
			Global.ModulateNodeAlpha(recording, 1);
		}
		else
		{
			Global.ModulateNodeAlpha(recording, 0);
		}
	}

	private void Flash() => light_visible = !light_visible;
}
