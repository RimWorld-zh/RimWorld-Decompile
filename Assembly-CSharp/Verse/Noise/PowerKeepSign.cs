using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9A RID: 3994
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x0600605B RID: 24667 RVA: 0x0030E8EC File Offset: 0x0030CCEC
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x0030E8F6 File Offset: 0x0030CCF6
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0030E914 File Offset: 0x0030CD14
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
