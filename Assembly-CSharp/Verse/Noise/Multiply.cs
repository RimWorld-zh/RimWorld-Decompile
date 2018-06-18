using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F92 RID: 3986
	public class Multiply : ModuleBase
	{
		// Token: 0x0600601F RID: 24607 RVA: 0x0030BE28 File Offset: 0x0030A228
		public Multiply() : base(2)
		{
		}

		// Token: 0x06006020 RID: 24608 RVA: 0x0030BE32 File Offset: 0x0030A232
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x0030BE50 File Offset: 0x0030A250
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
