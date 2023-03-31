using Godot;
using System;

public partial class Phone : AudioStreamPlayer
{
	public override async void _Ready()
	{
		if (Global.SkippedToOffice) return;
		GetNode<Button>("../HUD/MuteButton").Pressed += MuteButtonPressed;
		string ringPath = "res://assets/phone_ring.wav";
		string callPath = $"res://assets/call_{Global.GetNight()}.mp3";
		if (!FileAccess.FileExists(callPath)) return;
		Stream = GD.Load<AudioStream>(ringPath);
		Play();
		await ToSignal(this, "finished");
		if (Global.PrematurePhoneEnd) return;
		Stream = GD.Load<AudioStream>(callPath);
		Play();
	}

    public override void _Process(double delta)
    {
		Global.PhoneActive = Playing;
    }

    private void MuteButtonPressed()
	{
        Global.PrematurePhoneEnd = true;
		Stop();
	}
}
