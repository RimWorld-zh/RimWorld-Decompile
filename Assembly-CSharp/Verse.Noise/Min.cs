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
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
