using System;

namespace Verse.Noise
{
	// Token: 0x02000F9C RID: 3996
	public class SqueezeVertically : ModuleBase
	{
		// Token: 0x06006059 RID: 24665 RVA: 0x0030CB2D File Offset: 0x0030AF2D
		public SqueezeVertically(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0030CB48 File Offset: 0x0030AF48
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y * (double)this.factor, z);
		}

		// Token: 0x04003F22 RID: 16162
		private float factor;
	}
}
