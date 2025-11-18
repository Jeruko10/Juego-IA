using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility;

namespace Game;

[GlobalClass]
public partial class BotInputProvider() : VirtualInputProvider
{
	[Export] Board.Players self;
	[Export] float playSpeed = 1f;
	[Export] float courtesyDelay = 1f;

	public override void _Ready() => Board.State.TurnStarted += OnTurnStarted;

	async Task PlayTurn()
	{
		// USE THESE INPUT SIMULATION METHODS TO CONTROL THE BOT:
		//
		// SimulateHover(Vector2I?);
		// SimulateLeftClick(Vector2I);
		// SimulateRightClick(Vector2I);
		// SimulateHumanClick(Vector2I, bool, float, float)
		// SimulatePassTurn();

		await Wait(courtesyDelay);

		GoapBot.Waypoint[] waypoints = GoapBot.SetWaypoints();

		foreach (Minion minion in GetFriendlyMinions())
        {
			List<Vector2I> demandedClicks = HfsmBot.GetClicks(waypoints);
            
			foreach (Vector2I click in demandedClicks)
				await SimulateHumanClick(click);
        }

		SimulatePassTurn();
	}

	async Task SimulateHumanClick(Vector2I cell, bool rightClick = false, float hoverTime = 0.2f, float afterClickTime = 0.2f)
	{
		SimulateHover(cell);
		await Wait(hoverTime);

		if (rightClick) SimulateRightClick(cell);
		else SimulateLeftClick(cell);

		await Wait(afterClickTime);
	}

	async Task Wait(float seconds) => await Task.Delay((int)Mathf.Round(seconds * 1000 / playSpeed));

	List<Minion> GetEnemyMinions() => GetMinionsOwnedBy(Board.GetRival(self));

	List<Minion> GetFriendlyMinions() => GetMinionsOwnedBy(self);

	static List<Minion> GetMinionsOwnedBy(Board.Players player)
	{
		List<Minion> minions = [];

		foreach (Minion minion in Board.State.Minions)
			if (minion.Owner == player)
				minions.Add(minion);

		return minions;
	}

	async void OnTurnStarted(Board.Players newTurnOwner)
	{
		if (newTurnOwner != self) return;

		while (!InputHandler.InteractionEnabled)
			await Wait(0.1f); // Check each 0.1s if interaction is enabled

		await PlayTurn();
	}
}
