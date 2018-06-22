using System;

namespace Verse.Noise
{
	// Token: 0x02000F8E RID: 3982
	public class InverseLerp : ModuleBase
	{
		// Token: 0x0600603C RID: 24636 RVA: 0x0030DCB4 File Offset: 0x0030C0B4
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0030DCBE File Offset: 0x0030C0BE
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0030DCE0 File Offset: 0x0030C0E0
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

		// Token: 0x04003F1B RID: 16155
		private float from;

		// Token: 0x04003F1C RID: 16156
		private float to;
	}
}
