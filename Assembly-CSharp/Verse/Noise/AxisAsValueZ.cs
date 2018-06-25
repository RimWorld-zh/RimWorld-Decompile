using System;

namespace Verse.Noise
{
	// Token: 0x02000F76 RID: 3958
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06005F87 RID: 24455 RVA: 0x0030B4B2 File Offset: 0x003098B2
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0030B4BC File Offset: 0x003098BC
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
