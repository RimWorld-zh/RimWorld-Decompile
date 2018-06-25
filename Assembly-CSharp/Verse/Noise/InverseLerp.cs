using System;

namespace Verse.Noise
{
	// Token: 0x02000F93 RID: 3987
	public class InverseLerp : ModuleBase
	{
		// Token: 0x04003F26 RID: 16166
		private float from;

		// Token: 0x04003F27 RID: 16167
		private float to;

		// Token: 0x06006046 RID: 24646 RVA: 0x0030E578 File Offset: 0x0030C978
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0030E582 File Offset: 0x0030C982
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x0030E5A4 File Offset: 0x0030C9A4
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
	}
}
