namespace Verse.Noise
{
	public class Bend : ModuleBase
	{
		public Bend() : base(1)
		{
		}

		public Bend(float angle, float radius, ModuleBase input) : base(1)
		{
			base.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			return base.modules[0].GetValue(x, y, z);
		}
	}
}
