using System;

namespace Verse.Noise
{
	// Token: 0x02000F9B RID: 3995
	public class SqueezeVertically : ModuleBase
	{
		// Token: 0x06006080 RID: 24704 RVA: 0x0030ECAD File Offset: 0x0030D0AD
		public SqueezeVertically(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x0030ECC8 File Offset: 0x0030D0C8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y * (double)this.factor, z);
		}

		// Token: 0x04003F33 RID: 16179
		private float factor;
	}
}
