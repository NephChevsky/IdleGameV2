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
		public Number BaseHP { get; set; }
		public Number CurrentHP { get; set; }
		public Number MaxHP { get; set; }
		public float Position { get; set; }
		public int MovementSpeed { get; set; }

		public Entity(Number baseHP, int movementSpeed)
		{
			BaseHP = new(baseHP);
			CurrentHP = new(baseHP);
			MaxHP = new(baseHP);
			MovementSpeed = movementSpeed;
			Position = this is Player ? 0 : 1;
		}
	}
}
