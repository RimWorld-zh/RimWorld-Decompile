using System;

namespace Verse.Noise
{
	// Token: 0x02000F8D RID: 3981
	public class Filter : ModuleBase
	{
		// Token: 0x06006010 RID: 24592 RVA: 0x0030BB8A File Offset: 0x00309F8A
		public Filter() : base(1)
		{
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x0030BB94 File Offset: 0x00309F94
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x0030BBB8 File Offset: 0x00309FB8
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

		// Token: 0x04003F07 RID: 16135
		private float from;

		// Token: 0x04003F08 RID: 16136
		private float to;
	}
}
