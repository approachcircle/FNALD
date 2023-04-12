using Godot;
using System;

public partial class DoorController : Node
{
	TextureButton midDoor;
	CanvasGroup leftDoor;
	public override void _Ready()
	{
		midDoor = GetNode<TextureButton>("MidDoor");
		leftDoor = GetNode<CanvasGroup>("LeftDoor");
		midDoor.Pressed += MidDoorClicked;
		leftDoor.GetNode<Button>("Clickable").Pressed += LeftDoorClicked;
	}

	public override void _Process(double delta)
	{
		UpdateDoorVisuals();
	}

	private void MidDoorClicked()
	{
		Global.MidDoorClosed = !Global.MidDoorClosed;
	}

	private void LeftDoorClicked()
	{
		Global.LeftDoorClosed = !Global.LeftDoorClosed;
	}

	private void UpdateDoorVisuals()
	{
		leftDoor.Visible = Global.LeftDoorClosed;
		midDoor.Visible = !Global.MidDoorClosed;
	}
}
