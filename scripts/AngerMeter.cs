using Godot;
public partial class AngerMeter : Control
{
	bool emitted = false;
	Timer increaseTimer;

	[Signal]
	public delegate void AngryEventHandler();
	public override void _Ready()
	{
		GetNode<Label>("Anger").Text = "Anger: 0%";
		GetNode<Button>("../GetAngry").Pressed += GetAngry;
	}

	public override void _Process(double delta)
	{
		if (emitted) return;
		if (Global.InternalAnger <= 0)
		{
			GetAngry();
		}
		else
		{
			Global.InternalAnger -= Global.AngerRate;
		}
		Global.DisplayedAnger = 100 - (Global.InternalAnger / (Global.StartInternalAnger / 100));
		float r = (float) Global.DisplayedAnger / 100;
		float g = 1 - ((float) Global.DisplayedAnger / 100);
		Color angerColour = new Color(r, g, 0);
		Color green = new Color(0, 1, 0);
		Color yellowGreen = new Color(0.25f, 0.75f, 0);
		Color yellowRed = new Color(0.75f, 0.25f, 0);
		Color red = new Color(1, 0, 0);
		Label rate = GetNode<Label>("Rate");
		GetNode<Label>("Anger").Modulate = angerColour;
		switch (Global.AngerRate)
		{
			case 1:
				rate.Modulate = green;
				break;
			case 2:
				rate.Modulate = yellowGreen;
				break;
			case 3:
				rate.Modulate = yellowRed;
				break;
			case 4:
				rate.Modulate = red;
				break;
		}
		GetNode<Label>("Anger").Text = $"Anger: {Global.DisplayedAnger}%";
		GetNode<Label>("Rate").Text = $"Rate: {Global.AngerRate}";
	}

	private void GetAngry()
	{
		EmitSignal(nameof(Angry));
		emitted = true;
	}
}
