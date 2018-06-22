using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F86 RID: 3974
	public class Blend : ModuleBase
	{
		// Token: 0x0600600D RID: 24589 RVA: 0x0030D284 File Offset: 0x0030B684
		public Blend() : base(3)
		{
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x0030D28E File Offset: 0x0030B68E
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x0600600F RID: 24591 RVA: 0x0030D2B4 File Offset: 0x0030B6B4
		// (set) Token: 0x06006010 RID: 24592 RVA: 0x0030D2D1 File Offset: 0x0030B6D1
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

		// Token: 0x06006011 RID: 24593 RVA: 0x0030D2EC File Offset: 0x0030B6EC
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
