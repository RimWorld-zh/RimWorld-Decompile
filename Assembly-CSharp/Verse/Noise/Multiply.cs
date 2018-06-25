using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F96 RID: 3990
	public class Multiply : ModuleBase
	{
		// Token: 0x06006052 RID: 24658 RVA: 0x0030E54C File Offset: 0x0030C94C
		public Multiply() : base(2)
		{
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0030E556 File Offset: 0x0030C956
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x0030E574 File Offset: 0x0030C974
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
