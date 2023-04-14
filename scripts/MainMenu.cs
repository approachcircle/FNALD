using Godot;
using Sentry;
using System;
using System.Runtime.InteropServices;

public partial class MainMenu : Node
{
	bool loop_music = false;
	AudioStreamPlayer music;
	AudioStreamPlayer blip;
	Timer glitchTimer;
	Sprite2D background;

	public override void _Ready()
	{
		if (Global.GetNight() > 5 || Global.GetNight() < 1)
		{
			Global.SetNight(1);
		}
		Button continueButton = GetNode<Button>("Continue/ContinueButton");
		Button newGameButton = GetNode<Button>("NewGame/NewGameButton");
		CanvasGroup newGameDialog = GetNode<CanvasGroup>("ConfirmNewGame");
		music = GetNode<AudioStreamPlayer>("Music");
		blip = GetNode<AudioStreamPlayer>("Blip");
		glitchTimer = GetNode<Timer>("GlitchTimer");
		background = GetNode<Sprite2D>("Background");
		loop_music = true;
		GetNode<Label>("Continue/Night").Text = $"Night {Global.GetNight()}";
		continueButton.Pressed += Transition;
		newGameButton.Pressed += () => { newGameDialog.Visible = true; };
		newGameDialog.GetNode<Button>("YesButton").Pressed += () => { Global.SetNight(1); Transition(); };
		newGameDialog.GetNode<Button>("NoButton").Pressed += () => { newGameDialog.Visible = false; };
		continueButton.MouseEntered += ButtonHovered;
		newGameButton.MouseEntered += ButtonHovered;
		glitchTimer.Timeout += GlitchEnd;
	}

	public override void _Process(double delta)
	{
		if (loop_music)
		{
			if (!music.Playing)
				music.Play();
		}
		GlitchLogan();
	}

	private void GlitchLogan()
	{
		int start = 0;
		int end = 100;
		int result = new Random().Next(start, end);
		if (result == end / 2)
		{
			GetNode<Sprite2D>("Background").Visible = false;

			if (glitchTimer.IsStopped())
			{
				glitchTimer.Start();
			}
		}
	}

	private void Transition()
	{
		GetTree().ChangeSceneToFile("res://scenes/transition.tscn");
	}

	private void ButtonHovered()
	{
		blip.Play();
	}

	private void GlitchEnd()
	{
		background.Visible = true;
	}
}
