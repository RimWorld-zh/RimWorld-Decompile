#define DEBUG
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
			base.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(base.modules[0] != null);
			return 0.0 - base.modules[0].GetValue(x, y, z);
		}
	}
}
