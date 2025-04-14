using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	public class Enemy : Entity
	{
		public int Id { get; set; }

		public Enemy(Number hp, int movementSpeed) : base(hp, movementSpeed)
		{
		}

		public static Enemy GenerateEnemy(int level)
		{
			Enemy enemy = new(1 * Math.Pow(1.03, level - 1), 100);
			return enemy;
		}

		public static Enemy GenerateBoss(int level)
		{
			Enemy boss = new(5 * Math.Pow(1.03, level - 1), 0);
			return boss;
		}
	}
}
