using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F89 RID: 3977
	public class Add : ModuleBase
	{
		// Token: 0x06006011 RID: 24593 RVA: 0x0030DA71 File Offset: 0x0030BE71
		public Add() : base(2)
		{
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x0030DA7B File Offset: 0x0030BE7B
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0030DA98 File Offset: 0x0030BE98
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
