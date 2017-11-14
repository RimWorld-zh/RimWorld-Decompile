namespace Verse.Noise
{
	public class Blend : ModuleBase
	{
		public ModuleBase Controller
		{
			get
			{
				return base.modules[2];
			}
			set
			{
				base.modules[2] = value;
			}
		}

		public Blend()
			: base(3)
		{
		}

		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller)
			: base(3)
		{
			base.modules[0] = lhs;
			base.modules[1] = rhs;
			base.modules[2] = controller;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = base.modules[0].GetValue(x, y, z);
			double value2 = base.modules[1].GetValue(x, y, z);
			double position = (base.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
