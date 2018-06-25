using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F95 RID: 3989
	public class Max : ModuleBase
	{
		// Token: 0x0600604C RID: 24652 RVA: 0x0030E679 File Offset: 0x0030CA79
		public Max() : base(2)
		{
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0030E683 File Offset: 0x0030CA83
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x0030E6A0 File Offset: 0x0030CAA0
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
