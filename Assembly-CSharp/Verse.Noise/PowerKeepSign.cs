using System;

namespace Verse.Noise
{
	public class PowerKeepSign : ModuleBase
	{
		public PowerKeepSign() : base(2)
		{
		}

		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = base.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), base.modules[1].GetValue(x, y, z));
		}
	}
}
