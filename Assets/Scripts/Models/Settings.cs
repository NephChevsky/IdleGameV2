using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	public static class Settings
	{
		public static Game Game { get; set; } = new();
	}

	public class Game
	{
		public int TickRate = 1000;
		public float EntityCollisionOffset = 0.035f;
	}
}
