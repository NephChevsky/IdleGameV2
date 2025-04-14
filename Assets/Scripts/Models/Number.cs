using System;
using System.Diagnostics;

namespace Assets.Scripts.Models
{
	[DebuggerDisplay("{ToString()}")]
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

		public static Number operator -(Number a, Number b)
		{
			Number result = new(a);

			int exponentDiff = a.Exponent - b.Exponent;
			if (exponentDiff > 0)
			{
				result.Mantissa -= b.Mantissa * (float) Math.Pow(10, exponentDiff);
			}
			else if (exponentDiff < 0)
			{
				result.Mantissa = result.Mantissa / (float)Math.Pow(10, -exponentDiff) - b.Mantissa;
				result.Exponent = b.Exponent;
			}
			else
			{
				result.Mantissa -= b.Mantissa;
			}
			result.Normalize();
			return result;
		}

		public override string ToString()
		{
			return $"{Mantissa}e{Exponent}";
		}
	}
}
