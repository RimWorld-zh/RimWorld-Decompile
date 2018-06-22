using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F90 RID: 3984
	public class Max : ModuleBase
	{
		// Token: 0x06006042 RID: 24642 RVA: 0x0030DDB5 File Offset: 0x0030C1B5
		public Max() : base(2)
		{
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0030DDBF File Offset: 0x0030C1BF
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0030DDDC File Offset: 0x0030C1DC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
