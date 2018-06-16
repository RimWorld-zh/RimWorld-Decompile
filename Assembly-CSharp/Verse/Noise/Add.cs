using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F85 RID: 3973
	public class Add : ModuleBase
	{
		// Token: 0x06005FE0 RID: 24544 RVA: 0x0030B02D File Offset: 0x0030942D
		public Add() : base(2)
		{
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0030B037 File Offset: 0x00309437
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0030B054 File Offset: 0x00309454
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
