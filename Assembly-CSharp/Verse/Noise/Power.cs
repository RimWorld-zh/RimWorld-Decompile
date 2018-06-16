using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F95 RID: 3989
	public class Power : ModuleBase
	{
		// Token: 0x06006027 RID: 24615 RVA: 0x0030BE1F File Offset: 0x0030A21F
		public Power() : base(2)
		{
		}

		// Token: 0x06006028 RID: 24616 RVA: 0x0030BE29 File Offset: 0x0030A229
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0030BE48 File Offset: 0x0030A248
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
