using System;

namespace Verse.Noise
{
	// Token: 0x02000FA0 RID: 4000
	public class SqueezeVertically : ModuleBase
	{
		// Token: 0x04003F3E RID: 16190
		private float factor;

		// Token: 0x0600608A RID: 24714 RVA: 0x0030F571 File Offset: 0x0030D971
		public SqueezeVertically(ModuleBase input, float factor) : base(1)
		{
			this.modules[0] = input;
			this.factor = factor;
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0030F58C File Offset: 0x0030D98C
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y * (double)this.factor, z);
		}
	}
}
