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

		public Player(Number hp, Number attackDamage,int movementSpeed) : base(hp, attackDamage, movementSpeed)
		{
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
