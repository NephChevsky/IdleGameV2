using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Scripts.Models.Items
{
	[DebuggerDisplay("{Type.ToString()}")]
	public class Item
	{
		public Guid Id { get; set; }

		public ItemType Type { get; set; }

		public List<Affix> Affixes { get; set; } = new();

		public static Item Generate()
		{
			float rarity = UnityEngine.Random.Range(0f, 1f);
			Item item = new()
			{
				Id = Guid.NewGuid(),
				Type = ((ItemType[])Enum.GetValues(typeof(ItemType)))[UnityEngine.Random.Range(0, Enum.GetNames(typeof(ItemType)).Length)]
			};

			item.Affixes.Add(Affix.Generate());

			if (rarity > 0.8f)
			{
				item.Affixes.Add(Affix.Generate());
				if (rarity > 0.96f)
				{
					item.Affixes.Add(Affix.Generate());
					if (rarity > 0.992f)
					{
						item.Affixes.Add(Affix.Generate());
						if (rarity > 0.9984f)
						{
							item.Affixes.Add(Affix.Generate());
						}
					}
				}
			}
			return item;
		}
	}
}
