using System;

namespace Verse.Noise
{
	// Token: 0x02000F79 RID: 3961
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x04003ED9 RID: 16089
		public float span;

		// Token: 0x06005FA0 RID: 24480 RVA: 0x0030B446 File Offset: 0x00309846
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x0030B450 File Offset: 0x00309850
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0030B464 File Offset: 0x00309864
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}
	}
}
