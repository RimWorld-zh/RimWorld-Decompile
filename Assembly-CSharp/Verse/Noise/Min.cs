using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F91 RID: 3985
	public class Min : ModuleBase
	{
		// Token: 0x0600601C RID: 24604 RVA: 0x0030BD9C File Offset: 0x0030A19C
		public Min() : base(2)
		{
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0030BDA6 File Offset: 0x0030A1A6
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0030BDC4 File Offset: 0x0030A1C4
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
