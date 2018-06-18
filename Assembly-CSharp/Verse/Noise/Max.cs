using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F90 RID: 3984
	public class Max : ModuleBase
	{
		// Token: 0x06006019 RID: 24601 RVA: 0x0030BD11 File Offset: 0x0030A111
		public Max() : base(2)
		{
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0030BD1B File Offset: 0x0030A11B
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x0030BD38 File Offset: 0x0030A138
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
