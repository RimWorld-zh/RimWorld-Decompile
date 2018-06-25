using System;

namespace Verse.Noise
{
	// Token: 0x02000F76 RID: 3958
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06005F85 RID: 24453 RVA: 0x0030B6D5 File Offset: 0x00309AD5
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06005F86 RID: 24454 RVA: 0x0030B6E0 File Offset: 0x00309AE0
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
