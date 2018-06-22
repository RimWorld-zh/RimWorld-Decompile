using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F84 RID: 3972
	public class Add : ModuleBase
	{
		// Token: 0x06006007 RID: 24583 RVA: 0x0030D1AD File Offset: 0x0030B5AD
		public Add() : base(2)
		{
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x0030D1B7 File Offset: 0x0030B5B7
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x0030D1D4 File Offset: 0x0030B5D4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
