using System;

namespace Verse.Noise
{
	// Token: 0x02000F9B RID: 3995
	public class SqueezeVertically : ModuleBase
	{
		// Token: 0x06006057 RID: 24663 RVA: 0x0030CC09 File Offset: 0x0030B009
		public SqueezeVertically(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x0030CC24 File Offset: 0x0030B024
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y * (double)this.factor, z);
		}

		// Token: 0x04003F21 RID: 16161
		private float factor;
	}
}
