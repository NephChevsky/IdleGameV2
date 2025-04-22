using System;
using UnityEngine;

namespace Assets.Scripts.Engines
{
	public static class GraphicalEngine
	{
		public static GameObject AdventureTab;
		public static GameObject ArmoryTab;
		public static GameObject CraftTab;

		public static bool SalvageMode { get; set; } = false;
		public static bool SelectItemToCraftMode { get; set; } = false;
		public static Guid ItemToCraft { get; set; } = Guid.Empty;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void ResetStatics()
		{
			AdventureTab = null;
			ArmoryTab = null;
			CraftTab = null;

			SalvageMode = false;
			SelectItemToCraftMode = false;
			ItemToCraft = Guid.Empty;
		}

		public static void Init(GameObject adventureTab, GameObject armoryTab, GameObject craftTab)
		{
			AdventureTab = adventureTab;
			ArmoryTab = armoryTab;
			CraftTab = craftTab;

			SalvageMode = false;
			SelectItemToCraftMode = false;
			ItemToCraft = Guid.Empty;

			SwitchToTab("Adventure");
		}

		public static void ToggleSalvageMode()
		{
			SalvageMode = !SalvageMode;
		}

		public static void SwitchToTab(string name)
		{
			AdventureTab.SetActive(name == "Adventure");
			ArmoryTab.SetActive(name == "Armory");
			CraftTab.SetActive(name == "Craft");
		}
	}
}
