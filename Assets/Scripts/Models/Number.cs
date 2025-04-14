using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Models
{
	public class Number
	{
		public float Mantissa { get; set; } = 0;
		public int Exponent { get; set; } = 0;

		public Number(int value)
		{
			Mantissa = value;
			Exponent = 0;
			Normalize();
		}

		public Number(Number value)
		{
			Mantissa = value.Mantissa;
			Exponent = value.Exponent;
			Normalize();
		}

		public void Normalize()
		{
			while (Mantissa >= 10)
			{
				Mantissa /= 10;
				Exponent++;
			}
			while (Mantissa < 1 && Exponent > 0)
			{
				Mantissa *= 10;
				Exponent--;
			}
		}

		public static implicit operator Number(int value)
		{
			return new Number(value);
		}
	}
}
