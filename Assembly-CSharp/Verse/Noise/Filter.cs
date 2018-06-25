using System;

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
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double result;
			if (value >= (double)this.from && value <= (double)this.to)
			{
				result = 1.0;
			}
			else
			{
				result = 0.0;
			}
			return result;
		}
	}
}
