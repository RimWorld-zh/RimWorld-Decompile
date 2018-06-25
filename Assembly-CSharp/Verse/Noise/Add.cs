using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F88 RID: 3976
	public class Add : ModuleBase
	{
		// Token: 0x06006011 RID: 24593 RVA: 0x0030D82D File Offset: 0x0030BC2D
		public Add() : base(2)
		{
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x0030D837 File Offset: 0x0030BC37
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0030D854 File Offset: 0x0030BC54
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
