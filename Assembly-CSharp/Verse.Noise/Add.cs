#define DEBUG
using System.Diagnostics;

namespace Verse.Noise
{
	public class Add : ModuleBase
	{
		public Add() : base(2)
		{
		}

		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(base.modules[0] != null);
			Debug.Assert(base.modules[1] != null);
			return base.modules[0].GetValue(x, y, z) + base.modules[1].GetValue(x, y, z);
		}
	}
}
