using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	public class Entity
	{
		public float Position { get; set; }
		public Number BaseHP { get; set; }
		public Number CurrentHP { get; set; }
		public Number MaxHP { get; set; }
		public int AttackSpeed { get; set; }
		public float AttackTimer { get; set; }
		public int AttackRange { get; set; }
		public Number AttackDamage { get; set; }
		public int MovementSpeed { get; set; }

		public Entity(Number baseHP, Number attackDamage, int movementSpeed)
		{
			Position = this is Player ? 0 : 1;
			BaseHP = baseHP;
			CurrentHP = new(baseHP);
			MaxHP = new(baseHP);
			AttackSpeed = 100;
			AttackTimer = 1f * 100f / Settings.GameEngine.TickRate;
			AttackRange = 100;
			AttackDamage = attackDamage;
			MovementSpeed = movementSpeed;
		}
	}
}
