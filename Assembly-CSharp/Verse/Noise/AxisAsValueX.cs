using System;

namespace Verse.Noise
{
	// Token: 0x02000F72 RID: 3954
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06005F54 RID: 24404 RVA: 0x00308C91 File Offset: 0x00307091
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x00308C9C File Offset: 0x0030709C
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
