using System;

namespace Verse.Noise
{
	public class Power : ModuleBase
	{
		public Power() : base(2)
		{
		}

		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(base.modules[0].GetValue(x, y, z), base.modules[1].GetValue(x, y, z));
		}
	}
}
