using Assets.Scripts.Engines;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Items;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameEngine
{
	public static List<Item> Inventory { get; set; }
	public static Dictionary<AffixType, int> AffixShards { get; set; } = new();

    private static float TickTime = 1f / Settings.Game.TickRate;
    private static float CurrentTime { get; set; }
    private static float SaveTimer { get; set; }

	public static bool AutoSalvageWhite { get; private set; }
	public static bool AutoSalvageGreen { get; private set; }
	public static bool AutoSalvageBlue { get; private set; }
	public static bool AutoSalvagePurple { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetStatics()
	{
		TickTime = 1f / Settings.Game.TickRate;
		CurrentTime = 0f;
		SaveTimer = 0f;

		AutoSalvageWhite = false;
		AutoSalvageGreen = false;
		AutoSalvageBlue = false;
		AutoSalvagePurple = false;

		Inventory = new();
		AffixShards = new();
	}

	public static void Init(GameObject adventureTab, GameObject armoryTab, GameObject craftTab)
	{
		if (PlayerPrefs.HasKey("Inventory"))
		{
			string json = PlayerPrefs.GetString("Inventory");
			Inventory = JsonConvert.DeserializeObject<List<Item>>(json);
		}
		else
		{
			Inventory = new();
		}

		if (PlayerPrefs.HasKey("AffixShards"))
		{
			string json = PlayerPrefs.GetString("AffixShards");
			AffixShards = JsonConvert.DeserializeObject<Dictionary<AffixType, int>>(json);
		}
		else
		{
			AffixShards = new();
		}

		PlayerEngine.Init();
		MapEngine.Init();
		GraphicalEngine.Init(adventureTab, armoryTab, craftTab);

		AutoSalvageWhite = PlayerPrefs.GetInt("AutoSalvageWhite", 0) != 0;
		AutoSalvageGreen = PlayerPrefs.GetInt("AutoSalvageGreen", 0) != 0;
		AutoSalvageBlue = PlayerPrefs.GetInt("AutoSalvageBlue", 0) != 0;
		AutoSalvagePurple = PlayerPrefs.GetInt("AutoSalvagePurple", 0) != 0;
	}

    public static void Advance(float elapsedTime)
    {
		while (CurrentTime + elapsedTime >= TickTime)
        {
			MapEngine.Advance();

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
		if (PlayerEngine.Equipment.FirstOrDefault(x => x.Id == item.Id) != null)
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
			PlayerEngine.Equipment.Remove(item);
			Inventory.Add(item);
		}
	}

	private static void EquipItem(Item item)
	{
		Item equippedItem = null;
		if (item.Type == ItemType.Ring)
		{
			List<Item> rings = PlayerEngine.Equipment.Where(x => x.Type == item.Type).ToList();
			if (rings.Count == 2)
			{
				equippedItem = rings[0];
			}
		}
		else
		{
			equippedItem = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == item.Type);
		}
		if (equippedItem == null)
		{
			Inventory.Remove(item);
			PlayerEngine.Equipment.Add(item);
		}
		else
		{
			Inventory.Remove(item);
			PlayerEngine.Equipment.Remove(equippedItem);
			PlayerEngine.Equipment.Add(item);
			Inventory.Add(equippedItem);
		}
	}

	public static void SalvageItem(Item item)
	{
		foreach (Affix affix in item.Affixes)
		{
			if (!AffixShards.ContainsKey(affix.Type))
			{
				AffixShards.Add(affix.Type, 0);
			}
			AffixShards[affix.Type]++;
		}
		Inventory.Remove(item);
	}

	public static void ToggleAutoSalvageWhite()
	{
		AutoSalvageWhite = !AutoSalvageWhite;
	}

	public static void ToggleAutoSalvageGreen()
	{
		AutoSalvageGreen = !AutoSalvageGreen;
	}

	public static void ToggleAutoSalvageBlue()
	{
		AutoSalvageBlue = !AutoSalvageBlue;
	}

	public static void ToggleAutoSalvagePurple()
	{
		AutoSalvagePurple = !AutoSalvagePurple;
	}

	private static void Save()
    {
		PlayerEngine.Save();
		MapEngine.Save();

		PlayerPrefs.SetString("Inventory", JsonConvert.SerializeObject(Inventory));
		PlayerPrefs.SetString("AffixShards", JsonConvert.SerializeObject(AffixShards));

		PlayerPrefs.SetInt("AutoSalvageWhite", AutoSalvageWhite ? 1 : 0);
		PlayerPrefs.SetInt("AutoSalvageGreen", AutoSalvageGreen ? 1 : 0);
		PlayerPrefs.SetInt("AutoSalvageBlue", AutoSalvageBlue ? 1 : 0);
		PlayerPrefs.SetInt("AutoSalvagePurple", AutoSalvagePurple ? 1 : 0);
		PlayerPrefs.Save();
		Debug.Log("Game saved");
	}
}
