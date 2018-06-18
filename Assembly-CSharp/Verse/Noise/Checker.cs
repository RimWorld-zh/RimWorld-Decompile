using System;

namespace Verse.Noise
{
	// Token: 0x02000F74 RID: 3956
	public class Checker : ModuleBase
	{
		// Token: 0x06005F65 RID: 24421 RVA: 0x00309080 File Offset: 0x00307480
		public Checker() : base(0)
		{
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x0030908C File Offset: 0x0030748C
		public override double GetValue(double x, double y, double z)
		{
			int num = (int)Math.Floor(Utils.MakeInt32Range(x));
			int num2 = (int)Math.Floor(Utils.MakeInt32Range(y));
			int num3 = (int)Math.Floor(Utils.MakeInt32Range(z));
			return (((num & 1) ^ (num2 & 1) ^ (num3 & 1)) == 0) ? 1.0 : -1.0;
		}
	}
}
