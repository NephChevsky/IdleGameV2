using Assets.Scripts.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	public static Map Map { get; set; }
    public static List<Item> Inventory { get; set; }

    private static readonly float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; }
    private static float SaveTimer { get; set; }

    public static void Init()
    {
        int mapLevel = PlayerPrefs.GetInt("Map:Level", 1);
        int playerLevel = PlayerPrefs.GetInt("Map:Player:Level", 1);
		if (PlayerPrefs.HasKey("Inventory"))
		{
			string json = PlayerPrefs.GetString("Inventory");
			Inventory = JsonConvert.DeserializeObject<List<Item>>(json);
		}
		else
		{
			Inventory = new();
		}
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
		string json = JsonConvert.SerializeObject(Inventory);
		PlayerPrefs.SetString("Inventory", json);
		PlayerPrefs.Save();
		Debug.Log("Game saved");
	}
}
