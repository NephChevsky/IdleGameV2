using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Engines
{
	public static class GraphicalEngine
	{
		public static bool SalvageMode { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void ResetStatics()
		{
			SalvageMode = false;
		}

		public static void Init()
		{
			SalvageMode = false;
		}

		public static void ToggleSalvageMode()
		{
			SalvageMode = !SalvageMode;
		}
	}
}
