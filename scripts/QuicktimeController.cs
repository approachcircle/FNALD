using Godot;
using System;
using System.Collections.Generic;

public partial class QuicktimeController : CanvasGroup
{
	private static Dictionary<Key, int> TargetKeyMap { get; set; } = new Dictionary<Key, int>();
	public static event EventHandler QuicktimeFinished;
	public static event EventHandler QuicktimeStarted;
	private int TargetPresses { get; } = 30;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		foreach (Key key in TargetKeyMap.Keys)
		{
			GetNode<AnimatedSprite2D>($"Event{key}").Visible = true;
            GetNode<AnimatedSprite2D>($"Event{key}").Play();
            if (TargetKeyMap[key] >= TargetPresses)
			{
				EventFinished();
				return;
			}
			if (Input.IsKeyPressed(key))
			{
				TargetKeyMap[key]++;
			}
		}
	}

	public static void StartEvent(Key[] keys)
	{
		if (TargetKeyMap.Count > 0) return;
		QuicktimeStarted?.Invoke(null, EventArgs.Empty);
		foreach (Key key in keys)
		{
			if (key is not (Key.E or Key.Q))
			{
				throw new NotImplementedException($"a quicktime event for the key {key} doesn't exist");
			}
			else
			{
				TargetKeyMap.Add(key, 0);
            }
        }
	}

	private void EventFinished()
	{
		QuicktimeFinished?.Invoke(this, EventArgs.Empty);
		foreach (Key key in TargetKeyMap.Keys)
		{
            GetNode<AnimatedSprite2D>($"Event{key}").Visible = false;
			GetNode<AnimatedSprite2D>($"Event{key}").Stop();
        }
		TargetKeyMap.Clear();
    }
}
