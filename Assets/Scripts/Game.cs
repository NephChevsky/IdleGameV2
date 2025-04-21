using Assets.Scripts.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Game
{
	public static Map Map { get; set; }
	public static List<Item> Equipment { get; set; } = new();
	public static List<Item> Inventory { get; set; }

    private static readonly float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; }
    private static float SaveTimer { get; set; }
	public static bool SalvageMode { get; private set; }

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
		if (PlayerPrefs.HasKey("Equipment"))
		{
			string json = PlayerPrefs.GetString("Equipment");
			Equipment = JsonConvert.DeserializeObject<List<Item>>(json);
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

	public static void EquipOrUnequipItem(Item item)
	{
		if (Equipment.FirstOrDefault(x => x.Id == item.Id) != null)
		{
			UnequipItem(item);
		}
		else
		{
			EquipItem(item);
		}
	}

	private static void UnequipItem(Item item)
	{
		if (Inventory.Count < 90)
		{
			Equipment.Remove(item);
			Inventory.Add(item);
		}
	}

	private static void EquipItem(Item item)
	{
		Item equippedItem = null;
		if (item.Type == ItemType.Ring)
		{
			List<Item> rings = Equipment.Where(x => x.Type == item.Type).ToList();
			if (rings.Count == 2)
			{
				equippedItem = rings[0];
			}
		}
		else
		{
			equippedItem = Equipment.FirstOrDefault(x => x.Type == item.Type);
		}
		if (equippedItem == null)
		{
			Inventory.Remove(item);
			Equipment.Add(item);
		}
		else
		{
			Inventory.Remove(item);
			Equipment.Remove(equippedItem);
			Equipment.Add(item);
			Inventory.Add(equippedItem);
		}
	}

	public static void SalvageItem(Item item)
	{
		Inventory.Remove(item);
	}

	private static void Save()
    {
        PlayerPrefs.SetInt("Map:Level", Map.Level);
        PlayerPrefs.SetInt("Map:Player:Level", Map.Player.Level);
		string json = JsonConvert.SerializeObject(Inventory);
		PlayerPrefs.SetString("Inventory", json);
		json = JsonConvert.SerializeObject(Equipment);
		PlayerPrefs.SetString("Equipment", json);
		PlayerPrefs.Save();
		Debug.Log("Game saved");
	}

	public static void ToggleSalvageMode()
	{
		SalvageMode = !SalvageMode;
	}
}
