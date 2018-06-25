using System;

namespace Verse.Noise
{
	// Token: 0x02000F9E RID: 3998
	public class SqueezeHorizontally : ModuleBase
	{
		// Token: 0x04003F35 RID: 16181
		private float factor;

		// Token: 0x06006088 RID: 24712 RVA: 0x0030F2DB File Offset: 0x0030D6DB
		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x0030F2F8 File Offset: 0x0030D6F8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}
	}
}
