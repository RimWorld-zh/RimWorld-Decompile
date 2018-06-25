using System;

namespace Verse.Noise
{
	// Token: 0x02000F77 RID: 3959
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06005F87 RID: 24455 RVA: 0x0030B6F6 File Offset: 0x00309AF6
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0030B700 File Offset: 0x00309B00
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
