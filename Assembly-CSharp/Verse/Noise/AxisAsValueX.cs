using System;

namespace Verse.Noise
{
	// Token: 0x02000F75 RID: 3957
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06005F85 RID: 24453 RVA: 0x0030B491 File Offset: 0x00309891
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06005F86 RID: 24454 RVA: 0x0030B49C File Offset: 0x0030989C
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
