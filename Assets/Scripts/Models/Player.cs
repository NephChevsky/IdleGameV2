using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
	public class Player : Entity
	{

		public Player(int level) : base(100, 1, 100)
		{
		}

		public void Reset()
		{
			CurrentHP = new(MaxHP);
			Position = 0;
		}
	}
}
