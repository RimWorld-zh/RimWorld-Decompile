using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F94 RID: 3988
	public class Power : ModuleBase
	{
		// Token: 0x0600604E RID: 24654 RVA: 0x0030DF9F File Offset: 0x0030C39F
		public Power() : base(2)
		{
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x0030DFA9 File Offset: 0x0030C3A9
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0030DFC8 File Offset: 0x0030C3C8
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
