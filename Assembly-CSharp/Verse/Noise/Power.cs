using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F99 RID: 3993
	public class Power : ModuleBase
	{
		// Token: 0x06006058 RID: 24664 RVA: 0x0030E863 File Offset: 0x0030CC63
		public Power() : base(2)
		{
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x0030E86D File Offset: 0x0030CC6D
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0030E88C File Offset: 0x0030CC8C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
