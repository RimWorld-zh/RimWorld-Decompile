using System;

namespace Verse.Noise
{
	// Token: 0x02000F92 RID: 3986
	public class Filter : ModuleBase
	{
		// Token: 0x04003F24 RID: 16164
		private float from;

		// Token: 0x04003F25 RID: 16165
		private float to;

		// Token: 0x06006043 RID: 24643 RVA: 0x0030E4F2 File Offset: 0x0030C8F2
		public Filter() : base(1)
		{
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0030E4FC File Offset: 0x0030C8FC
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006045 RID: 24645 RVA: 0x0030E520 File Offset: 0x0030C920
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
