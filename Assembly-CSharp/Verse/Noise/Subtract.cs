using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9D RID: 3997
	public class Subtract : ModuleBase
	{
		// Token: 0x0600605B RID: 24667 RVA: 0x0030CB75 File Offset: 0x0030AF75
		public Subtract() : base(2)
		{
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x0030CB7F File Offset: 0x0030AF7F
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0030CB9C File Offset: 0x0030AF9C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
