using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F91 RID: 3985
	public class Max : ModuleBase
	{
		// Token: 0x0600601B RID: 24603 RVA: 0x0030BC35 File Offset: 0x0030A035
		public Max() : base(2)
		{
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x0030BC3F File Offset: 0x0030A03F
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0030BC5C File Offset: 0x0030A05C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
