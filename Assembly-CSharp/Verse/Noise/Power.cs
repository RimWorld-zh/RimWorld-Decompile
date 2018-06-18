using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F94 RID: 3988
	public class Power : ModuleBase
	{
		// Token: 0x06006025 RID: 24613 RVA: 0x0030BEFB File Offset: 0x0030A2FB
		public Power() : base(2)
		{
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0030BF05 File Offset: 0x0030A305
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006027 RID: 24615 RVA: 0x0030BF24 File Offset: 0x0030A324
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
