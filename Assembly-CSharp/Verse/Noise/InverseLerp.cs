using System;

namespace Verse.Noise
{
	// Token: 0x02000F8E RID: 3982
	public class InverseLerp : ModuleBase
	{
		// Token: 0x06006013 RID: 24595 RVA: 0x0030BC10 File Offset: 0x0030A010
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x0030BC1A File Offset: 0x0030A01A
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0030BC3C File Offset: 0x0030A03C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double num = (value - (double)this.from) / (double)(this.to - this.from);
			double result;
			if (num < 0.0)
			{
				result = 0.0;
			}
			else if (num > 1.0)
			{
				result = 1.0;
			}
			else
			{
				result = num;
			}
			return result;
		}

		// Token: 0x04003F09 RID: 16137
		private float from;

		// Token: 0x04003F0A RID: 16138
		private float to;
	}
}
