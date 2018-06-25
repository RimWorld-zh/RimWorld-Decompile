using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8A RID: 3978
	public class Blend : ModuleBase
	{
		// Token: 0x06006017 RID: 24599 RVA: 0x0030D904 File Offset: 0x0030BD04
		public Blend() : base(3)
		{
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0030D90E File Offset: 0x0030BD0E
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06006019 RID: 24601 RVA: 0x0030D934 File Offset: 0x0030BD34
		// (set) Token: 0x0600601A RID: 24602 RVA: 0x0030D951 File Offset: 0x0030BD51
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

		// Token: 0x0600601B RID: 24603 RVA: 0x0030D96C File Offset: 0x0030BD6C
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
