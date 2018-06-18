using System;

namespace Verse.Noise
{
	// Token: 0x02000F71 RID: 3953
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06005F52 RID: 24402 RVA: 0x00308D6D File Offset: 0x0030716D
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x00308D78 File Offset: 0x00307178
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
