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
		if (Global.KnockingLeft)
		{
			Global.KnockingLeft = false;
			GetNode<AudioStreamPlayer>("KnockLeft").Play();
		}
		if (Global.KnockingMid)
		{
			Global.KnockingMid = false;
            GetNode<AudioStreamPlayer>("KnockMid").Play();
        }
	}

	private void MidDoorClicked()
	{
		if (Global.ClosedDoors.Contains(Room.Mid))
		{
			Global.AngerRate--;
			Global.ClosedDoors.Remove(Room.Mid);
		}
		else
		{
			Global.AngerRate++;
			Global.ClosedDoors.Add(Room.Mid);
		}
	}

	private void LeftDoorClicked()
	{
        if (Global.ClosedDoors.Contains(Room.Left))
        {
            Global.AngerRate--;
            Global.ClosedDoors.Remove(Room.Left);
        }
        else
        {
            Global.AngerRate++;
            Global.ClosedDoors.Add(Room.Left);
        }
    }

	private void UpdateDoorVisuals()
	{
		if (Global.ClosedDoors.Contains(Room.Left))
			Global.ModulateNodeAlpha(leftDoor, 1);
		else
			Global.ModulateNodeAlpha(leftDoor, 0);

		if (Global.ClosedDoors.Contains(Room.Mid))
			Global.ModulateNodeAlpha(midDoor, 0);
		else
			Global.ModulateNodeAlpha(midDoor, 1);
	}
}
