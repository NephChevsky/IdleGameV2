using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	public static class Settings
	{
		public static GameEngine GameEngine { get; set; } = new();
	}

	public class GameEngine
	{
		public int TickRate = 100;
		public float EntityCollisionOffset = 0.035f;
	}
}
