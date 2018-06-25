using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F98 RID: 3992
	public class Power : ModuleBase
	{
		// Token: 0x06006058 RID: 24664 RVA: 0x0030E61F File Offset: 0x0030CA1F
		public Power() : base(2)
		{
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x0030E629 File Offset: 0x0030CA29
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0030E648 File Offset: 0x0030CA48
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
