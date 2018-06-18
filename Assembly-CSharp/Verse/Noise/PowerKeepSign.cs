using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F95 RID: 3989
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x06006028 RID: 24616 RVA: 0x0030BF84 File Offset: 0x0030A384
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0030BF8E File Offset: 0x0030A38E
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x0030BFAC File Offset: 0x0030A3AC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
