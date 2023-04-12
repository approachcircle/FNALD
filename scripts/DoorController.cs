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
		Global.ModulateNodeAlpha(leftDoor.GetNode<Button>("Clickable"), 0);
	}

	public override void _Process(double delta)
	{
		UpdateDoorVisuals();
	}

	private void MidDoorClicked()
	{
		if (Global.MidDoorClosed) Global.AngerRate--; else Global.AngerRate++;
		Global.MidDoorClosed = !Global.MidDoorClosed;
	}

	private void LeftDoorClicked()
	{
		if (Global.LeftDoorClosed) Global.AngerRate--; else Global.AngerRate++;
		Global.LeftDoorClosed = !Global.LeftDoorClosed;
	}

	private void UpdateDoorVisuals()
	{
		if (Global.LeftDoorClosed)
			Global.ModulateNodeAlpha(leftDoor, 1);
		else
			Global.ModulateNodeAlpha(leftDoor, 0);

		if (Global.MidDoorClosed)
			Global.ModulateNodeAlpha(midDoor, 0);
		else
			Global.ModulateNodeAlpha(midDoor, 1);
	}
}
