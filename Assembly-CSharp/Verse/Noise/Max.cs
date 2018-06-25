using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F94 RID: 3988
	public class Max : ModuleBase
	{
		// Token: 0x0600604C RID: 24652 RVA: 0x0030E435 File Offset: 0x0030C835
		public Max() : base(2)
		{
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0030E43F File Offset: 0x0030C83F
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x0030E45C File Offset: 0x0030C85C
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
