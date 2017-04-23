using System;
using System.Diagnostics;

namespace Verse.Noise
{
	public class Subtract : ModuleBase
	{
		public Subtract() : base(2)
		{
		}

		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
