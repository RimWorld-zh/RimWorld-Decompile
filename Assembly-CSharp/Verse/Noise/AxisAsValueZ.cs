using System;

namespace Verse.Noise
{
	// Token: 0x02000F72 RID: 3954
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06005F54 RID: 24404 RVA: 0x00308D8E File Offset: 0x0030718E
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x00308D98 File Offset: 0x00307198
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
