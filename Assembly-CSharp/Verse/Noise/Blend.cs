using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F86 RID: 3974
	public class Blend : ModuleBase
	{
		// Token: 0x06005FE4 RID: 24548 RVA: 0x0030B1E0 File Offset: 0x003095E0
		public Blend() : base(3)
		{
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0030B1EA File Offset: 0x003095EA
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06005FE6 RID: 24550 RVA: 0x0030B210 File Offset: 0x00309610
		// (set) Token: 0x06005FE7 RID: 24551 RVA: 0x0030B22D File Offset: 0x0030962D
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

		// Token: 0x06005FE8 RID: 24552 RVA: 0x0030B248 File Offset: 0x00309648
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
