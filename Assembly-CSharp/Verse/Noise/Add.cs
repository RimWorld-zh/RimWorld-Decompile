using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F84 RID: 3972
	public class Add : ModuleBase
	{
		// Token: 0x06005FDE RID: 24542 RVA: 0x0030B109 File Offset: 0x00309509
		public Add() : base(2)
		{
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x0030B113 File Offset: 0x00309513
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x0030B130 File Offset: 0x00309530
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
