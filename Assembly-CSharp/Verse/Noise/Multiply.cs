using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F92 RID: 3986
	public class Multiply : ModuleBase
	{
		// Token: 0x06006048 RID: 24648 RVA: 0x0030DECC File Offset: 0x0030C2CC
		public Multiply() : base(2)
		{
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x0030DED6 File Offset: 0x0030C2D6
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0030DEF4 File Offset: 0x0030C2F4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
