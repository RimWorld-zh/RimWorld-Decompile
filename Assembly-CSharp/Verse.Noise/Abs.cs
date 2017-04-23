using System;
using System.Diagnostics;

namespace Verse.Noise
{
	public class Abs : ModuleBase
	{
		public Abs() : base(1)
		{
		}

		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
