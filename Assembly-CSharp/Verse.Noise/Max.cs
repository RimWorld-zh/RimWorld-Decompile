using System;

namespace Verse.Noise
{
	public class Max : ModuleBase
	{
		public Max()
			: base(2)
		{
		}

		public Max(ModuleBase lhs, ModuleBase rhs)
			: base(2)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = base.modules[0].GetValue(x, y, z);
			double value2 = base.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
