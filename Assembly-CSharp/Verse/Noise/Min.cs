using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F96 RID: 3990
	public class Min : ModuleBase
	{
		// Token: 0x0600604F RID: 24655 RVA: 0x0030E704 File Offset: 0x0030CB04
		public Min() : base(2)
		{
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0030E70E File Offset: 0x0030CB0E
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0030E72C File Offset: 0x0030CB2C
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
