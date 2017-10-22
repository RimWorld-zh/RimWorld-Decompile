using System;

namespace Verse.Noise
{
	public class Abs : ModuleBase
	{
		public Abs() : base(1)
		{
		}

		public Abs(ModuleBase input) : base(1)
		{
			base.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(base.modules[0].GetValue(x, y, z));
		}
	}
}
