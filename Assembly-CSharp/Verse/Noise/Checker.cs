using System;

namespace Verse.Noise
{
	// Token: 0x02000F78 RID: 3960
	public class Checker : ModuleBase
	{
		// Token: 0x06005F98 RID: 24472 RVA: 0x0030B7A4 File Offset: 0x00309BA4
		public Checker() : base(0)
		{
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x0030B7B0 File Offset: 0x00309BB0
		public override double GetValue(double x, double y, double z)
		{
			int num = (int)Math.Floor(Utils.MakeInt32Range(x));
			int num2 = (int)Math.Floor(Utils.MakeInt32Range(y));
			int num3 = (int)Math.Floor(Utils.MakeInt32Range(z));
			return (((num & 1) ^ (num2 & 1) ^ (num3 & 1)) == 0) ? 1.0 : -1.0;
		}
	}
}
