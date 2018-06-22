using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F83 RID: 3971
	public class Abs : ModuleBase
	{
		// Token: 0x06006004 RID: 24580 RVA: 0x0030D151 File Offset: 0x0030B551
		public Abs() : base(1)
		{
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x0030D15B File Offset: 0x0030B55B
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x0030D170 File Offset: 0x0030B570
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
