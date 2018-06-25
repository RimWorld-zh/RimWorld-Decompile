using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F99 RID: 3993
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x0600605B RID: 24667 RVA: 0x0030E6A8 File Offset: 0x0030CAA8
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x0030E6B2 File Offset: 0x0030CAB2
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0030E6D0 File Offset: 0x0030CAD0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
