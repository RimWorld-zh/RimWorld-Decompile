using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F87 RID: 3975
	public class Blend : ModuleBase
	{
		// Token: 0x06005FE6 RID: 24550 RVA: 0x0030B104 File Offset: 0x00309504
		public Blend() : base(3)
		{
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x0030B10E File Offset: 0x0030950E
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06005FE8 RID: 24552 RVA: 0x0030B134 File Offset: 0x00309534
		// (set) Token: 0x06005FE9 RID: 24553 RVA: 0x0030B151 File Offset: 0x00309551
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				Debug.Assert(value != null);
				this.modules[2] = value;
			}
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x0030B16C File Offset: 0x0030956C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			Debug.Assert(this.modules[2] != null);
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
