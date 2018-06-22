using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F91 RID: 3985
	public class Min : ModuleBase
	{
		// Token: 0x06006045 RID: 24645 RVA: 0x0030DE40 File Offset: 0x0030C240
		public Min() : base(2)
		{
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x0030DE4A File Offset: 0x0030C24A
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0030DE68 File Offset: 0x0030C268
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
