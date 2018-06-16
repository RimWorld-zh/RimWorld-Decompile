using System;

namespace Verse.Noise
{
	// Token: 0x02000F8F RID: 3983
	public class InverseLerp : ModuleBase
	{
		// Token: 0x06006015 RID: 24597 RVA: 0x0030BB34 File Offset: 0x00309F34
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0030BB3E File Offset: 0x00309F3E
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0030BB60 File Offset: 0x00309F60
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

		// Token: 0x04003F0A RID: 16138
		private float from;

		// Token: 0x04003F0B RID: 16139
		private float to;
	}
}
