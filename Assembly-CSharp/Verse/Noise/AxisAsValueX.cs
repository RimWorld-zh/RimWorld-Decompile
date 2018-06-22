using System;

namespace Verse.Noise
{
	// Token: 0x02000F71 RID: 3953
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06005F7B RID: 24443 RVA: 0x0030AE11 File Offset: 0x00309211
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x0030AE1C File Offset: 0x0030921C
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
