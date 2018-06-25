using System;

namespace Verse.Noise
{
	// Token: 0x02000F7E RID: 3966
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x04003EE4 RID: 16100
		public float span;

		// Token: 0x06005FAA RID: 24490 RVA: 0x0030BD0A File Offset: 0x0030A10A
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x0030BD14 File Offset: 0x0030A114
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0030BD28 File Offset: 0x0030A128
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}
	}
}
