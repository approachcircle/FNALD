using Godot;
using System;
using System.Collections.Generic;

/**
 * IMPORTANT: ensure you distinguish between references to these different nodes:
 * 
 *		"Camera"/"Phys(ical) Camera(s)": the physical Camera2D node that the player observes from.
 *		
 *		"Cameras": the in-game cameras that the player uses to spot monsters.
 */
public partial class Office : Node2D
{
	[Export]
	float speed = 1000.0f;
	float fastSpeed;
	float velocity = 1.0f;
	List<Camera2D> physCameras = new List<Camera2D>();
	RigidBody2D HUD;
	CanvasGroup hitboxes;
	Timer progression;
	int physCameraIndex = 0;

	public override void _Ready()
	{
		Global.PlayerDisabled = false;
		Global.PrematurePhoneEnd = false;
		Global.InternalAnger = Global.StartInternalAnger;
		Global.RemainingTime = 0;
		Global.CamerasEnabled = true;
		// Global.MonstersEnabled = false;
		fastSpeed = speed * 1.5f;
		hitboxes = GetNode<CanvasGroup>("Hitboxes");
		HUD = GetNode<RigidBody2D>("HUD");
		progression = GetNode<Timer>("Progression");
		hitboxes.Visible = true;
		Global.ModulateNodeAlpha(hitboxes, 0);
		physCameras.Add(GetNode<Camera2D>("HUD/Camera"));
		physCameras.Add(GetNode<Camera2D>("DebugCamera"));
		physCameras[physCameraIndex].Enabled = true;
		progression.Timeout += HourPassed;
		GetNode<AngerMeter>("HUD/AngerMeter").Angry += AngerTriggered;
		GetNode<Button>("HUD/NextHour").Pressed += HourPassed;
		GetNode<DistractionController>("DistractionController").Distracted += CloseCameras;
		QuicktimeController.QuicktimeStarted += (_, _) => CloseCameras();
		GetNode<Button>("HUD/Quicktime").Pressed += () =>
		{
			QuicktimeController.StartEvent(new Key[] { Key.Q, Key.E });
		};
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("switch_camera"))
			SwitchCamera();
		if (Input.IsActionJustPressed("skip_night"))
			Global.Time = 6;
		CheckTime();
		GetNode<Label>("HUD/Time").Text = $"{Global.Time} AM";
		if (GetNode<AudioStreamPlayer>("Phone").Playing)
		{
			GetNode<Button>("HUD/MuteButton").Visible = true;
			GetNode<Label>("HUD/MuteText").Visible = true;
		}
		else
		{
			GetNode<Button>("HUD/MuteButton").Visible = false;
			GetNode<Label>("HUD/MuteText").Visible = false;
		}
		GetNode<Label>("HUD/MinutesOfHour").Text = $"Time to next hour: {progression.TimeLeft}";
		Global.RemainingTime = progression.TimeLeft;

		// ensure cameras are definitely invisible when they should be
		if (Global.Distracted && Global.Cameras == Global.CamerasState.Up ||
			Global.Distracted && HUD.GetNode<Control>("Cameras").Visible ||
			Global.Cameras == Global.CamerasState.Down)
		{
			HUD.GetNode<Control>("Cameras").Visible = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleScrolling(delta);
		HandleCameraAccess();
	}

	private bool MouseIsOver(Control node)
	{
		Camera2D camera = physCameras[physCameraIndex];
		Vector2 mousePosition = camera.GetViewport().GetMousePosition();
		bool hasPoint = node.GetGlobalRect().HasPoint(mousePosition);
		return hasPoint;
	}

	private bool MouseIsOverAny(Control[] nodes)
	{
		Camera2D camera = physCameras[physCameraIndex];
		Vector2 mousePosition = camera.GetViewport().GetMousePosition();
		bool hasPoint = false;
		foreach (Control node in nodes)
		{
			hasPoint = node.GetGlobalRect().HasPoint(mousePosition);
			if (hasPoint) break;
		}
		return hasPoint;
	}

	private void HandleScrolling(double delta)
	{
		if (Global.PlayerDisabled) return;
		bool scrolling = false;
		ColorRect slowLeft = hitboxes.GetNode<ColorRect>("SlowLeft");
		ColorRect slowRight = hitboxes.GetNode<ColorRect>("SlowRight");
		ColorRect fastLeft = hitboxes.GetNode<ColorRect>("FastLeft");
		ColorRect fastRight = hitboxes.GetNode<ColorRect>("FastRight");
		if (MouseIsOverAny(new Control[] { slowLeft, slowRight, fastLeft, fastRight }))
			scrolling = true;
		if (MouseIsOver(slowLeft))
			velocity = -speed;
		else if (MouseIsOver(slowRight))
			velocity = speed;
		else if (MouseIsOver(fastLeft))
			velocity = -fastSpeed;
		else if (MouseIsOver(fastRight))
			velocity = fastSpeed;
		if (Global.Cameras is Global.CamerasState.Up)
			velocity = 1;
		if (scrolling)
			HUD.MoveAndCollide(new Vector2((float)(velocity * delta), 0));
	}

	private void HandleCameraAccess()
	{
		if (Global.PlayerDisabled || !Global.CamerasEnabled) return;
		Sprite2D CAB = HUD.GetNode<Sprite2D>("CamerasAccessButton");
		if (MouseIsOver(hitboxes.GetNode<ColorRect>("CameraAccess"))) {
			if (CAB.Modulate.A == 0)
				return;
			ToggleCameras();
			Global.ModulateNodeAlpha(CAB, 0);
		}
		else if (!MouseIsOver(hitboxes.GetNode<ColorRect>("CameraAccess")))
			Global.ModulateNodeAlpha(CAB, 1);
	}

	private async void ToggleCameras()
	{
		GetNode<AudioStreamPlayer>("CamerasSound").Play();
		if (Global.Cameras == Global.CamerasState.Up)
		{
			Global.Cameras = Global.CamerasState.Down;
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").Visible = true;
			HUD.GetNode<Control>("Cameras").Visible = false;
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").Play();
			await ToSignal(HUD.GetNode<AnimatedSprite2D>("CamerasOpening"), "animation_finished");
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").Visible = false;
			Global.AngerRate--;
		}
		else
		{
			Global.Cameras = Global.CamerasState.Up;
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").Visible = true;
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").PlayBackwards();
			await ToSignal(HUD.GetNode<AnimatedSprite2D>("CamerasOpening"), "animation_finished");
			HUD.GetNode<AnimatedSprite2D>("CamerasOpening").Visible = false;
			HUD.GetNode<Control>("Cameras").Visible = true;
			Global.AngerRate++;
		}
	}

	private void SwitchCamera()
	{
		physCameras[physCameraIndex].Enabled = false;
		if (physCameraIndex + 1 >= physCameras.Count)
			physCameraIndex = 0;
		else
			physCameraIndex++;
		physCameras[physCameraIndex].Enabled = true;
		if (physCameras[physCameraIndex].Name.ToString().Contains("Debug"))
			Global.ModulateNodeAlpha(hitboxes, 1);
		else
			Global.ModulateNodeAlpha(hitboxes, 0);
	}

	private void CheckTime()
	{
		if (Global.Time == 6)
			EndNight();
	}

	private void EndNight()
	{
		CloseCameras();
		GetTree().ChangeSceneToFile("res://scenes/night_end.tscn");
	}

	private void HourPassed()
	{
		if (Global.Time == 12)
			Global.Time = 1;
		else
			Global.Time++;
	}

	private void AngerTriggered()
	{
		CloseCameras();
		Global.PrematurePhoneEnd = true;
		GetNode<AudioStreamPlayer>("Phone").Stop();
	}

	private void CloseCameras()
	{
		if (Global.Cameras is Global.CamerasState.Up)
			ToggleCameras();
	}
}
