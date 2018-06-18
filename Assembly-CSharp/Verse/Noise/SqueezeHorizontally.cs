using System;

namespace Verse.Noise
{
	// Token: 0x02000F9A RID: 3994
	public class SqueezeHorizontally : ModuleBase
	{
		// Token: 0x06006055 RID: 24661 RVA: 0x0030CBB7 File Offset: 0x0030AFB7
		public SqueezeHorizontally(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0030CBD4 File Offset: 0x0030AFD4
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * (double)this.factor, y, z * (double)this.factor);
		}

		// Token: 0x04003F20 RID: 16160
		private float factor;
	}
}
