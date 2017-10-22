namespace Verse.Noise
{
	public class Filter : ModuleBase
	{
		private float from;

		private float to;

		public Filter() : base(1)
		{
		}

		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			base.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = base.modules[0].GetValue(x, y, z);
			return (!(value >= (double)this.from) || !(value <= (double)this.to)) ? 0.0 : 1.0;
		}
	}
}
