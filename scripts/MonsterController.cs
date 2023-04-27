using Godot;
using System;
using System.Collections.Generic;

public partial class MonsterController : Node
{
	public static Monster[] Monsters { get; } = {
		new Cameron(),
		new Harry(),
		new Millie()
	};

	public static List<Monster> PeekingLeft { get; } = new List<Monster>();
	public static List<Monster> LastLeft { get; } = new List<Monster>();
	public static List<Monster> PeekingMid { get; } = new List<Monster>();
	public static List<Monster> LastMid { get; } = new List<Monster>();
	List<Monster> WaitingAttackers { get; } = new List<Monster>();
	public static Monster AttackingMonster { get; private set; }

	Timer attackTimer;
	bool attackTimerStarted = false;
	Timer jumpTimer;
	bool jumpTimerStarted = false;

    public override void _Ready()
	{
		GetNode<Timer>("Movement").Timeout += UpdateMonsters;
		attackTimer = GetNode<Timer>("AttackTimer");
		attackTimer.Timeout += AttackTimeout;
		jumpTimer = GetNode<Timer>("JumpTimer");
		jumpTimer.Timeout += Jumpscare;
	}

	public override void _Process(double delta)
    {
		foreach (Monster monster in Monsters)
		{
			UpdateStatus(monster);
			CheckPeeking(monster);
			CheckAwaitingAttacks(monster);
		}
	}

	private void UpdateMonsters()
	{
		if (!Global.MonstersEnabled) return;
		foreach (Monster monster in Monsters)
		{
			monster.Roll();
			monster.Move();
		}
	}

	private void CheckPeeking(Monster monster)
	{
		if (monster.Room is not Room.Mid)
		{
            if (PeekingMid.Contains(monster))
			{
				PeekingMid.Remove(monster);
			}
        }
		if (monster.Room is not Room.Left)
		{
			if (PeekingLeft.Contains(monster))
			{
                PeekingLeft.Remove(monster);
			}
		}
		if (monster.Room is Room.Mid)
		{
			if (!PeekingMid.Contains(monster))
			{
                PeekingMid.Add(monster);
				LastMid.Add(monster);
            }
		}
		if (monster.Room is Room.Left)
		{
            if (!PeekingLeft.Contains(monster))
			{
                PeekingLeft.Add(monster);
				LastLeft.Add(monster);
            }
		}
	}

	private void UpdateStatus(Monster monster)
	{
        if (!monster.CanAttack && monster.Room is not Room.Office)
        {
            if (attackTimerStarted)
                attackTimer.Stop();
            monster.State = MonsterState.Idle;
            if (WaitingAttackers.Contains(monster))
                WaitingAttackers.Remove(monster);
        }
		if (!monster.CanAttack && monster.IsPeeking)
		{
            if (attackTimerStarted)
                attackTimer.Stop();
            monster.State = MonsterState.Blocked;
            if (WaitingAttackers.Contains(monster))
                WaitingAttackers.Remove(monster);
        }
        if (monster.CanAttack)
		{
			if (Global.Cameras is Global.CamerasState.Up)
			{
                monster.State = MonsterState.Attacking;
				return;
            }
			monster.State = MonsterState.CanAttack;
			if (!attackTimerStarted)
			{
				attackTimer.Start();
				attackTimerStarted = true;
			}
			if (!WaitingAttackers.Contains(monster))
				WaitingAttackers.Add(monster);
		}

		if (monster.Room == Room.Office)
			monster.State = MonsterState.Attacking;
	}

	private void AttackTimeout()
	{
		foreach (Monster attacker in WaitingAttackers)
			attacker.Goto(Room.Office);
	}

	private void CheckAwaitingAttacks(Monster monster)
	{
		if (monster.State is MonsterState.Attacking)
		{
			AttackingMonster = monster;
			if (Global.Cameras is Global.CamerasState.Down)
				Jumpscare();
			else
                jumpTimer.Start();
		}
	}

	private void Jumpscare()
	{
		Global.AttackType = AttackType.Monster;
		GetTree().ChangeSceneToFile("res://scenes/jumpscare.tscn");
	}
}
