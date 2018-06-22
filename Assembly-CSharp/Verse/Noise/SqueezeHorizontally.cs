using System;

namespace Verse.Noise
{
	// Token: 0x02000F9A RID: 3994
	public class SqueezeHorizontally : ModuleBase
	{
		// Token: 0x0600607E RID: 24702 RVA: 0x0030EC5B File Offset: 0x0030D05B
		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x0030EC78 File Offset: 0x0030D078
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}

		// Token: 0x04003F32 RID: 16178
		private float factor;
	}
}
