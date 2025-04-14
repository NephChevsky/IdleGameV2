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

		public Enemy(int id, Number hp, int movementSpeed) : base(hp, movementSpeed)
		{
			Id = id;
		}
	}
}
