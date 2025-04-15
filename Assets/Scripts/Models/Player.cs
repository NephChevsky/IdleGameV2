using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			CurrentXP = CurrentXP - MaxXP;
			MaxXP = MaxXP * 1.5;
		}

		public void Reset()
		{
			CurrentHP = new(MaxHP);
			Position = 0;
		}
	}
}
