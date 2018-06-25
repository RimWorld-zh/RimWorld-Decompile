using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F88 RID: 3976
	public class Abs : ModuleBase
	{
		// Token: 0x0600600E RID: 24590 RVA: 0x0030DA15 File Offset: 0x0030BE15
		public Abs() : base(1)
		{
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x0030DA1F File Offset: 0x0030BE1F
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x0030DA34 File Offset: 0x0030BE34
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
