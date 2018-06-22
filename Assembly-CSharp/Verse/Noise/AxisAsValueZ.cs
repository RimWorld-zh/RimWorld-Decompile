using System;

namespace Verse.Noise
{
	// Token: 0x02000F72 RID: 3954
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06005F7D RID: 24445 RVA: 0x0030AE32 File Offset: 0x00309232
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x0030AE3C File Offset: 0x0030923C
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
