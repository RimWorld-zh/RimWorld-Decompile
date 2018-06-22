using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F95 RID: 3989
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x06006051 RID: 24657 RVA: 0x0030E028 File Offset: 0x0030C428
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0030E032 File Offset: 0x0030C432
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0030E050 File Offset: 0x0030C450
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
