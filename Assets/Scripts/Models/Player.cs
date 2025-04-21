using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
	public class Player : Entity
	{
		public int Level { get; set; } = 1;
		public Number CurrentXP { get; set; } = 0;
		public Number MaxXP { get; set; } = 20;

		public Player(int level) : base(100, 1, 100)
		{
			Level = level;
		}

		public void LevelUp()
		{
			Level++;
			CurrentXP -= MaxXP;
			MaxXP = 20 * Math.Pow(1.5, Level);
		}

		public void Reset()
		{
			CurrentHP = new(MaxHP);
			Position = 0;
		}
	}
}
