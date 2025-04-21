using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Affix
	{
		public AffixType Type { get; set; }
		public Number Value { get; set; }

		public static Affix Generate()
		{
			Affix affix = new()
			{
				Type = ((AffixType[])Enum.GetValues(typeof(AffixType)))[UnityEngine.Random.Range(0, Enum.GetNames(typeof(AffixType)).Length)],
				Value = UnityEngine.Random.Range(1, 5) / 100f 
			};
			return affix;
		}
	}
}
