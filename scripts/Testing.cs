using Godot;
using System;

public partial class Testing : Control
{
	public override void _Ready()
	{
		var passFail = GetNode<Label>("PassFail");
		long nightSet = 3;
		GetNode<Label>("Set").Text += nightSet.ToString();
		Global.SetNight(nightSet);
		long nightGet = Global.GetNight();
		GetNode<Label>("Get").Text += nightGet.ToString();
		if (nightSet == nightGet)
		{
			passFail.Text = "PASS";
		}
		else
		{
			passFail.Text = "FAIL";
		}
	}

	public override void _Process(double delta)
	{
	}
}
