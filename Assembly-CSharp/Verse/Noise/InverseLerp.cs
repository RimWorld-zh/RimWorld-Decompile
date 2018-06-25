using System;

namespace Verse.Noise
{
	// Token: 0x02000F92 RID: 3986
	public class InverseLerp : ModuleBase
	{
		// Token: 0x04003F1E RID: 16158
		private float from;

		// Token: 0x04003F1F RID: 16159
		private float to;

		// Token: 0x06006046 RID: 24646 RVA: 0x0030E334 File Offset: 0x0030C734
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0030E33E File Offset: 0x0030C73E
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x0030E360 File Offset: 0x0030C760
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
