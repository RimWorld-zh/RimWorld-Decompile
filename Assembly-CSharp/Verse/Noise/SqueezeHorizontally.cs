using System;

namespace Verse.Noise
{
	// Token: 0x02000F9F RID: 3999
	public class SqueezeHorizontally : ModuleBase
	{
		// Token: 0x04003F3D RID: 16189
		private float factor;

		// Token: 0x06006088 RID: 24712 RVA: 0x0030F51F File Offset: 0x0030D91F
		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x0030F53C File Offset: 0x0030D93C
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}
	}
}
