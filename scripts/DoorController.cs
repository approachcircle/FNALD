using Godot;
using System;

public partial class DoorController : Node
{
	ColorRect midDoorHitbox;
	ColorRect leftDoorHitbox;
	public override void _Ready()
	{
		GetNode<TextureButton>("MidDoor").Pressed += MidDoorClicked;
		GetNode<Button>("LeftDoor/Clickable").Pressed += LeftDoorClicked;
	}

	public override void _Process(double delta)
	{
		
	}

	private void MidDoorClicked()
	{

	}

	private void LeftDoorClicked()
	{

	}
}
