using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9C RID: 3996
	public class Subtract : ModuleBase
	{
		// Token: 0x06006059 RID: 24665 RVA: 0x0030CC51 File Offset: 0x0030B051
		public Subtract() : base(2)
		{
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0030CC5B File Offset: 0x0030B05B
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x0030CC78 File Offset: 0x0030B078
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
