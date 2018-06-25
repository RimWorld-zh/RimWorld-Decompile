using System;

namespace Verse.Noise
{
	// Token: 0x02000F7D RID: 3965
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x04003EDC RID: 16092
		public float span;

		// Token: 0x06005FAA RID: 24490 RVA: 0x0030BAC6 File Offset: 0x00309EC6
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x0030BAD0 File Offset: 0x00309ED0
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0030BAE4 File Offset: 0x00309EE4
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}
	}
}
