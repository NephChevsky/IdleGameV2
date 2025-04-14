using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	public static Map Map { get; set; }

    private static readonly float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; }

    public static void Init()
    {
		Map = new(1);
    }

    public static void Advance(float elapsedTime)
    {
		while (CurrentTime + elapsedTime >= TickTime)
        {
			Map.Advance();

			CurrentTime += TickTime;
            CurrentTime %= TickTime;
            elapsedTime -= TickTime;
		}

        CurrentTime += elapsedTime;
	}
}
