using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F96 RID: 3990
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x0600602A RID: 24618 RVA: 0x0030BEA8 File Offset: 0x0030A2A8
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0030BEB2 File Offset: 0x0030A2B2
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x0030BED0 File Offset: 0x0030A2D0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
