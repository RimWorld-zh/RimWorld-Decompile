using System;

namespace Verse.Noise
{
	// Token: 0x02000F79 RID: 3961
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x06005F77 RID: 24439 RVA: 0x003093A2 File Offset: 0x003077A2
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x003093AC File Offset: 0x003077AC
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x003093C0 File Offset: 0x003077C0
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x04003EC7 RID: 16071
		public float span;
	}
}
