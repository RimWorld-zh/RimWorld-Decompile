#define DEBUG
using System;
using System.Diagnostics;

namespace Verse.Noise
{
	public class Min : ModuleBase
	{
		public Min() : base(2)
		{
		}

		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(base.modules[0] != null);
			Debug.Assert(base.modules[1] != null);
			double value = base.modules[0].GetValue(x, y, z);
			double value2 = base.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
