namespace Verse.Noise
{
	public class SqueezeHorizontally : ModuleBase
	{
		private float factor;

		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			base.modules[0] = input;
			this.factor = factor;
		}

		public override double GetValue(double x, double y, double z)
		{
			return base.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}
	}
}
