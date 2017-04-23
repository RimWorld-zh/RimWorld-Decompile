using System;
using System.Diagnostics;

namespace Verse.Noise
{
	public class Invert : ModuleBase
	{
		public Invert() : base(1)
		{
		}

		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
