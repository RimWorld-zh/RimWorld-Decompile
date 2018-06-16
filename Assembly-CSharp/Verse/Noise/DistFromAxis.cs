using System;

namespace Verse.Noise
{
	// Token: 0x02000F7A RID: 3962
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x06005F79 RID: 24441 RVA: 0x003092C6 File Offset: 0x003076C6
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x003092D0 File Offset: 0x003076D0
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x003092E4 File Offset: 0x003076E4
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x04003EC8 RID: 16072
		public float span;
	}
}
