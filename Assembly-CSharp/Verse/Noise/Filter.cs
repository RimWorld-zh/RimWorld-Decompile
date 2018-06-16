using System;

namespace Verse.Noise
{
	// Token: 0x02000F8E RID: 3982
	public class Filter : ModuleBase
	{
		// Token: 0x06006012 RID: 24594 RVA: 0x0030BAAE File Offset: 0x00309EAE
		public Filter() : base(1)
		{
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0030BAB8 File Offset: 0x00309EB8
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x0030BADC File Offset: 0x00309EDC
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

		// Token: 0x04003F08 RID: 16136
		private float from;

		// Token: 0x04003F09 RID: 16137
		private float to;
	}
}
