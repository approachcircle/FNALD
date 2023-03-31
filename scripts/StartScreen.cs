using Godot;
using System.Collections.Generic;

public partial class StartScreen : Node
{
	List<CanvasGroup> texts = new List<CanvasGroup>();
	AnimationPlayer animator;
	int text = 0;
	
	public override async void _Ready()
	{
		animator = GetNode<AnimationPlayer>("Animator");
		texts.Add(GetNode<CanvasGroup>("Warning"));
		texts.Add(GetNode<CanvasGroup>("Information"));
		animator.Play("fade_in");
		await ToSignal(animator, "animation_finished");
	}

	public override async void _Process(double delta)
	{
		if (Input.IsActionJustPressed("next_menu")) {
			animator.Play("fade_out");
			await ToSignal(animator, "animation_finished");
			texts[text].Visible = false;
			if (text < texts.Count - 1) {
				text++;
				texts[text].Visible = true;
				animator.Play("fade_in");
			}
			else
			{
				GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
				// GetTree().ChangeSceneToFile("res://scenes/testing.tscn");
			}
		}
	}
}
