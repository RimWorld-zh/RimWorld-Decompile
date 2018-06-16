using System;

namespace Verse.Noise
{
	// Token: 0x02000F75 RID: 3957
	public class Checker : ModuleBase
	{
		// Token: 0x06005F67 RID: 24423 RVA: 0x00308FA4 File Offset: 0x003073A4
		public Checker() : base(0)
		{
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x00308FB0 File Offset: 0x003073B0
		public override double GetValue(double x, double y, double z)
		{
			int num = (int)Math.Floor(Utils.MakeInt32Range(x));
			int num2 = (int)Math.Floor(Utils.MakeInt32Range(y));
			int num3 = (int)Math.Floor(Utils.MakeInt32Range(z));
			return (((num & 1) ^ (num2 & 1) ^ (num3 & 1)) == 0) ? 1.0 : -1.0;
		}
	}
}
