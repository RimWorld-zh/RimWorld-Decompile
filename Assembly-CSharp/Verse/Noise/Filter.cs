using System;

namespace Verse.Noise
{
	// Token: 0x02000F8D RID: 3981
	public class Filter : ModuleBase
	{
		// Token: 0x06006039 RID: 24633 RVA: 0x0030DC2E File Offset: 0x0030C02E
		public Filter() : base(1)
		{
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0030DC38 File Offset: 0x0030C038
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0030DC5C File Offset: 0x0030C05C
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

		// Token: 0x04003F19 RID: 16153
		private float from;

		// Token: 0x04003F1A RID: 16154
		private float to;
	}
}
