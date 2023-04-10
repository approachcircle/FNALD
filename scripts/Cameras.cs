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

	}

	private void FlashLight()
	{
        Sprite2D recording = GetNode<Sprite2D>("Recording");
        if (light_visible)
            Global.ModulateNodeAlpha(recording, 1);
        else
            Global.ModulateNodeAlpha(recording, 0);
    }
}
