using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	public class Player : Entity
	{
		public Number CurrentXP { get; set; } = 0;

		public Player(Number hp, Number attackDamage,int movementSpeed) : base(hp, attackDamage, movementSpeed)
		{
		}

		public void Reset()
		{
			CurrentHP = new(MaxHP);
			Position = 0;
		}
	}
}
