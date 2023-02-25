using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Util
{

    internal class MathUtil
    {

		private static readonly Random RANDOM = new();

		private MathUtil() { }

		public static int RandomNumber(int max)
		{
			return RANDOM.Next(max);
		}

		public static int RandomNumber(int min, int max)
        {
			return RANDOM.Next(min, max);
        }

		public static void MoveUD(double current, double end, double smoothSpeed, double minSpeed, out double result)
		{
			double movement = (end - current) * smoothSpeed;
			if (movement > 0)
			{
				movement = Math.Max(minSpeed, movement);
				movement = Math.Min(end - current, movement);
			}
			else if (movement < 0)
			{
				movement = Math.Min(-minSpeed, movement);
				movement = Math.Max(end - current, movement);
			}
			current += movement;
			result = current;
		}

	}

}
