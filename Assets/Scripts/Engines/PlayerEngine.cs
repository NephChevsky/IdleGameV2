using Assets.Scripts.Models;
using Assets.Scripts.Models.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engines
{
	public static class PlayerEngine
	{
		public static int Level { get; set; } = 1;
		public static Number CurrentXP { get; set; } = 0;
		public static Number MaxXP { get; set; } = 20;
		public static List<Item> Equipment { get; set; } = new();
		public static Dictionary<AffixType, int> AffectedAttributePoints { get; set; } = new();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void ResetStatics()
		{
			Level = 1;
			CurrentXP = 0;
			MaxXP = 0;
			Equipment.Clear();
			AffectedAttributePoints.Clear();
		}

		public static void Init()
		{
			Level = PlayerPrefs.GetInt("Player:Level", 1);

			if (PlayerPrefs.HasKey("Player:Equipment"))
			{
				string json = PlayerPrefs.GetString("Player:Equipment");
				Equipment = JsonConvert.DeserializeObject<List<Item>>(json);
			}
			else
			{
				Equipment = new();
			}

			if (PlayerPrefs.HasKey("Map:Player:CurrentXP"))
			{
				string json = PlayerPrefs.GetString("Map:Player:CurrentXP");
				CurrentXP = JsonConvert.DeserializeObject<Number>(json);
			}
			else
			{
				CurrentXP = 0;
			}

			MaxXP = 20 * Math.Pow(1.5, Level);

			if (PlayerPrefs.HasKey("Player:AffectedAttributePoints"))
			{
				string json = PlayerPrefs.GetString("Player:AffectedAttributePoints");
				AffectedAttributePoints = JsonConvert.DeserializeObject<Dictionary<AffixType, int>>(json);
			}
			else
			{
				AffectedAttributePoints = new();
				foreach (AffixType affix in Enum.GetValues(typeof(AffixType)))
				{
					AffectedAttributePoints.Add(affix, 0);
				}
			}
		}

		public static void GainXP(Number xp)
		{
			CurrentXP += xp;
			if (CurrentXP >= MaxXP)
			{
				LevelUp();
			}
		}

		public static void LevelUp()
		{
			Level++;
			CurrentXP -= MaxXP;
			MaxXP = 20 * Math.Pow(1.5, Level);
		}

		public static Number GetAffixTypeBonusFromEquipment(AffixType affixType)
		{
			Number value = 0f;
			foreach (Item item in Equipment)
			{
				foreach (Affix affix in item.Affixes)
				{
					if (affix.Type == affixType)
					{
						value += affix.Value;
					}
				}
			}
			return value;
		}

		public static Number GetAffixTypeBonusFromAttributes(AffixType affixType)
		{
			return AffectedAttributePoints[affixType] / 100f;
		}

		public static void Save()
		{
			PlayerPrefs.SetInt("Player:Level", Level);
			PlayerPrefs.SetString("Player:CurrentXP", JsonConvert.SerializeObject(CurrentXP));
			PlayerPrefs.SetString("Player:Equipment", JsonConvert.SerializeObject(Equipment));
			PlayerPrefs.SetString("Player:AffectedAttributePoints", JsonConvert.SerializeObject(AffectedAttributePoints));
			PlayerPrefs.Save();
		}
	}
}
