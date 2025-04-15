using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	public static Map Map { get; set; }

    private static readonly float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; }
    private static float SaveTimer { get; set; }

    public static void Init()
    {
        int mapLevel = PlayerPrefs.GetInt("Map:Level", 1);
        int playerLevel = PlayerPrefs.GetInt("Map:Player:Level", 1);
		Map = new(mapLevel, playerLevel);
    }

    public static void Advance(float elapsedTime)
    {
		while (CurrentTime + elapsedTime >= TickTime)
        {
			Map.Advance();

            SaveTimer += TickTime;
            if (SaveTimer > 30f)
            {
                SaveTimer = 0f;
                Save();
            }

			CurrentTime += TickTime;
            CurrentTime %= TickTime;
            elapsedTime -= TickTime;
		}

        CurrentTime += elapsedTime;
	}

    private static void Save()
    {
        PlayerPrefs.SetInt("Map:Level", Map.Level);
        PlayerPrefs.SetInt("Map:Player:Level", Map.Player.Level);
        PlayerPrefs.Save();
    }
}
