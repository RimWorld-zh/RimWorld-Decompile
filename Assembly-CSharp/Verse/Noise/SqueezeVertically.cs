using System;

namespace Verse.Noise
{
	// Token: 0x02000F9F RID: 3999
	public class SqueezeVertically : ModuleBase
	{
		// Token: 0x04003F36 RID: 16182
		private float factor;

		// Token: 0x0600608A RID: 24714 RVA: 0x0030F32D File Offset: 0x0030D72D
		public SqueezeVertically(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0030F348 File Offset: 0x0030D748
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y * (double)this.factor, z);
		}
	}
}
