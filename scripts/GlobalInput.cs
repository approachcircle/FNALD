using Godot;
using System;

public partial class GlobalInput : Node
{
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("exit"))
			GetTree().Quit();
		if (Input.IsActionJustPressed("enter_office"))
		{
			Global.SkippedToOffice = true;
			GetTree().ChangeSceneToFile("res://scenes/office.tscn");
		}
	}
}
