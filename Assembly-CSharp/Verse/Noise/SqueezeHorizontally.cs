using System;

namespace Verse.Noise
{
	// Token: 0x02000F9B RID: 3995
	public class SqueezeHorizontally : ModuleBase
	{
		// Token: 0x06006057 RID: 24663 RVA: 0x0030CADB File Offset: 0x0030AEDB
		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x0030CAF8 File Offset: 0x0030AEF8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}

		// Token: 0x04003F21 RID: 16161
		private float factor;
	}
}
