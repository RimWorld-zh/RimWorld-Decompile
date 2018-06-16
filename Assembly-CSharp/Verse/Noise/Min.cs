using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F92 RID: 3986
	public class Min : ModuleBase
	{
		// Token: 0x0600601E RID: 24606 RVA: 0x0030BCC0 File Offset: 0x0030A0C0
		public Min() : base(2)
		{
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x0030BCCA File Offset: 0x0030A0CA
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006020 RID: 24608 RVA: 0x0030BCE8 File Offset: 0x0030A0E8
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
