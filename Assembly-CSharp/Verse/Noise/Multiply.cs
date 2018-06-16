using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F93 RID: 3987
	public class Multiply : ModuleBase
	{
		// Token: 0x06006021 RID: 24609 RVA: 0x0030BD4C File Offset: 0x0030A14C
		public Multiply() : base(2)
		{
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0030BD56 File Offset: 0x0030A156
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0030BD74 File Offset: 0x0030A174
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
